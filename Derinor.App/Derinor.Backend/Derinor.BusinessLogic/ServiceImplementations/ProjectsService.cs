using Derinor.Application.ServiceInterfaces;
using Derinor.Common.RequestDTOs;
using Derinor.Common.ResponseDTOs;
using Derinor.DataAccess.RepositoryInterfaces;
using Derinor.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Derinor.Application.ServiceImplementations
{
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        public ProjectsService(IProjectsRepository projectsRepository, IHttpClientFactory httpClientFactory, IConfiguration configuration, IUserRepository userRepository) {

            _projectsRepository = projectsRepository;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<List<ProjectHomeResponseDTO>> AllProjects(string? searchData)
        {
            var fetchedProjects = await _projectsRepository.AllProjects(searchData);

            var fetchedProjectsDTO = fetchedProjects.Select(p => new ProjectHomeResponseDTO
            {
                projectOwner = p.ProjectOwner,
                projectName = p.ProjectName,
                projectDescription = p.ProjectDescription,

            }).ToList(); ;

            return fetchedProjectsDTO;
        }

        public async Task CreateProject(CreateProjectDetailsRequestDTO projectDetails,int userID)
        {
            var newProject = new Projects
            {
                ProjectOwner = projectDetails.projectOwner,
                ProjectName = projectDetails.projectName,
                ProjectDescription = projectDetails.projectDescription,
                UserID = userID
            };

            var insertedProjectData = await _projectsRepository.InsertProject(newProject);

            var newBranches = new ProjectBranches
            {
                ProjectProductionBranch = projectDetails.projectBranches.projectProductionBranch,
                ProjectRepository = projectDetails.projectBranches.projectRepository,
                ProjectID = insertedProjectData.ProjectID,
            };

            await _projectsRepository.InsertBranches(newBranches);
        }

        public async Task<List<GithubCommitResponseDTO>> GetGithubCommits(int userID, int projectID)
        {
            var detailsForFetching = await _userRepository.GetFetchingDetails(userID, projectID);

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", detailsForFetching.Users.GithubAccessToken);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("derinor-app");


            var owner = detailsForFetching.ProjectOwner;
            var branch = detailsForFetching.ProjectBranches.FirstOrDefault();
            var repo = branch.ProjectRepository;
            var since = branch.StartingDate;
            var until = DateTime.Now;

            var commits = new List<GithubCommitResponseDTO>();
            var page = 1;
            bool hasMorePages = true;

            while (hasMorePages == true)
            {

                var url = $"https://api.github.com/repos/{owner}/{repo}/commits" + $"?since={since}&until={until}&per_page=100&page={page}";

                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                var pageCommits = JsonSerializer.Deserialize<List<GithubCommitResponseDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                commits.AddRange(pageCommits);

                var linkHeader = response.Headers.GetValues("Link").FirstOrDefault();
                hasMorePages = linkHeader != null && linkHeader.Contains("rel=\"next\"");

                page++;
            }

            return commits;
        }

        public async Task<GeneratedReportDataRequestDTO> GenerateReport(int userID, int projectID)
        {
            var commits = await GetGithubCommits(userID, projectID);
            var fetchDetails = await _userRepository.GetFetchingDetails(userID, projectID);

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fetchDetails.Users.GithubAccessToken);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("derinor-app");

            var owner = fetchDetails.ProjectOwner;
            var branch = fetchDetails.ProjectBranches.FirstOrDefault();
            var repo = branch.ProjectRepository;
            var since = branch.StartingDate;
            var until = DateTime.Now;

            var analysisInputs = new List<GithubCommitAnalysisRequestDTO>();

            foreach(var commit in commits)
            {
                var detailUrl = $"https://api.github.com/repos/{owner}/{repo}/commits/{commit.Sha}";

                var resp = await client.GetAsync(detailUrl);

                using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
                var root = doc.RootElement;

                var patches = root.GetProperty("files").EnumerateArray().Select(f => f.GetProperty("patch").GetString());

                var fullPatch = string.Join("\n\n", patches);

                analysisInputs.Add(new GithubCommitAnalysisRequestDTO
                {
                    Sha = commit.Sha,
                    MessageDescription = commit.CommitMessage.MessageDescription,
                    Patch = fullPatch
                });
            }

            return new GeneratedReportDataRequestDTO
            {
                AllCommitsData = analysisInputs
            };


        }

        public async Task<GeminiDataResponseDTO> GetGeminiData(int userID, int projectID)
        {
            var commitDetails = await GenerateReport(userID, projectID);

            var sb = new StringBuilder();
            sb.AppendLine("You are an expert changelog reports writer for a startup.");
            sb.AppendLine("Read the following commits and code diffs,");
            sb.AppendLine("then give me a concise summary of notable changes made based on the commit messages and code changes");
            sb.AppendLine("The summary should include Finished Work and development period at the top,than each notable change (commit message and its code differences) include title and description about what was changed that will be understandable to non technical person");
            sb.AppendLine("It should be stuctured and formatted in understandable way, you can include things like features developed, bug fixes, make sure to not include sensitive informations from the code or commits");
            sb.AppendLine("Here is an example of how it should look like - Finished Work (Development Period: 03/09 – 03/16)\r\n\r\nNew Features\r\nEnhanced Restaurant Search Filters\r\nWhat it does: Users can now quickly narrow down restaurant options by selecting filters like cuisine type, price range, and customer ratings.\r\nImpact: Makes it easier for users to find exactly what they're in the mood for.\r\nTechnical Note: We updated our search algorithms and improved the data indexing process to ensure the filters work faster and more accurately.\r\nCustomer Photo Reviews\r\nWhat it does: Customers can attach photos to their reviews, offering a more\r\n");
            sb.AppendLine();

            foreach (var commit in commitDetails.AllCommitsData)
            {
                sb.AppendLine($"Commit SHA: {commit.Sha}");
                sb.AppendLine($"Message: {commit.MessageDescription}");
                sb.AppendLine("Diff:");
                sb.AppendLine(commit.Patch);
                sb.AppendLine(new string('-', 40));
            }

            var promptText = sb.ToString();

            
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["Gemini:ApiKey"]);

            var requestBody = new
            {
                model = "gemini-pro", 
                messages = new[]
                {
                  new { role = "system", content = "You are a helpful assistant." },
                  new { role = "user",   content = promptText }
                },
                temperature = 0.2
            };

            var jsonPayload = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            
            var response = await client.PostAsync("https://api.generative.google.com/v1beta2/models/gemini-pro:generateMessage", content);
            var rawJson = await response.Content.ReadAsStringAsync();

            
            using var doc = JsonDocument.Parse(rawJson);
            var reply = doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetString();

            var returnedGeminiData = new GeminiDataResponseDTO
            {
                GeminiMessage = reply,
            };

            return returnedGeminiData;


        }

        public async Task<List<GithubRepositoryResponseDTO>> FetchRepositories(int userID)
        {
            var userData = await _userRepository.GetUsernameByUserID(userID);
            var githubUsername = userData.GithubUsername;
            var githubAccessToken = userData.GithubAccessToken;

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", githubAccessToken);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("derinor-app");

            var url = $"https://api.github.com/users/{githubUsername}/repos";

            var response = await client.GetAsync(url);

            var jsonString = await response.Content.ReadAsStringAsync();


            var repositories = JsonSerializer.Deserialize<List<GithubRepositoryResponseDTO>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            
            var repositoryDto = repositories.Select(repo => new GithubRepositoryResponseDTO
            {
                Id = repo.Id,
                Name = repo.Name

            }).ToList();

            return repositoryDto;
        }

        public async Task<List<GithubBranchesResponseDTO>> FetchBranches(int userID, string repositoryName)
        {
            var userData = await _userRepository.GetUsernameByUserID(userID);
            var githubUsername = userData.GithubUsername;
            var githubAccessToken = userData.GithubAccessToken;

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", githubAccessToken);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("derinor-app");

            var url = $"https://api.github.com/repos/{githubUsername}/{repositoryName}/branches";

            var response = await client.GetAsync(url);

            var jsonString = await response.Content.ReadAsStringAsync();

            var branches = JsonSerializer.Deserialize<List<GithubBranchesResponseDTO>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });


            var branchesDTO = branches.Select(repo => new GithubBranchesResponseDTO
            {
                name = repo.name,
                githubCommitDTO = new GithubCommitDTO
                {
                    sha = repo.githubCommitDTO.sha,
                }

            }).ToList();

            return branchesDTO;

        }


    }
};
