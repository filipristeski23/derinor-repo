using Derinor.Application.ServiceInterfaces;
using Derinor.Common.RequestDTOs;
using Derinor.Common.ResponseDTOs;
using Derinor.DataAccess.RepositoryInterfaces;
using Derinor.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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

        public ProjectsService(IProjectsRepository projectsRepository,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IUserRepository userRepository)
        {
            _projectsRepository = projectsRepository;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<List<ProjectHomeResponseDTO>> AllProjects(string? search, int userID)
        {
            var fetchedProjects = await _projectsRepository.AllProjects(search, userID);
            return fetchedProjects.Select(p => new ProjectHomeResponseDTO
            {
                projectOwner = p.ProjectOwner,
                projectName = p.ProjectName,
                projectDescription = p.ProjectDescription,
                projectID = p.ProjectID
            }).ToList();
        }

        public async Task CreateProject(CreateProjectDetailsRequestDTO projectDetails, int userID)
        {
            var newProject = new Projects
            {
                ProjectOwner = projectDetails.projectOwner,
                ProjectName = projectDetails.projectName,
                ProjectDescription = projectDetails.projectDescription,
                UserID = userID
            };
            var insertedProjectData = await _projectsRepository.InsertProject(newProject);
            await _projectsRepository.InsertBranches(new ProjectBranches
            {
                ProjectProductionBranch = projectDetails.projectBranches.projectProductionBranch,
                ProjectRepository = projectDetails.projectBranches.projectRepository,
                ProjectID = insertedProjectData.ProjectID,
            });
        }

        public async Task<List<GithubCommitResponseDTO>> GetGithubCommits(int userID, int projectID, DateTime startDate, DateTime endDate, Projects details)
        {
            var branch = details.ProjectBranches.FirstOrDefault() ?? throw new InvalidOperationException("No branches selected for this project");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", details.Users.GithubAccessToken);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("derinor-app");

            var commits = new List<GithubCommitResponseDTO>();
            var page = 1;
            const int maxPages = 50;
            bool hasMore;
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            do
            {
                var since = startDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                var until = endDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

                var url = $"https://api.github.com/repos/{details.Users.GithubUsername}/{branch.ProjectRepository}/commits?" +
                            $"sha={Uri.EscapeDataString(branch.ProjectProductionBranch)}&" +
                            $"since={since}&until={until}&" +
                            $"per_page=100&page={page}";
                try
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrWhiteSpace(json)) break;
                    var pageCommits = JsonSerializer.Deserialize<List<GithubCommitResponseDTO>>(json, jsonOptions);

                    if (pageCommits == null || pageCommits.Count == 0)
                    {
                        hasMore = false;
                        break;
                    }
                    if (commits.Any() && commits.Last().Sha == pageCommits.First().Sha)
                    {
                        break;
                    }
                    commits.AddRange(pageCommits);
                    var linkHeader = response.Headers.GetValues("Link").FirstOrDefault();
                    hasMore = linkHeader?.Contains("rel=\"next\"") == true;
                }
                catch
                {
                    hasMore = false;
                    break;
                }
            }
            while (hasMore && ++page <= maxPages);
            return commits;
        }

        public async Task<GeneratedReportDataRequestDTO> GenerateReport(int userID, GenerateReportRequestDTO request, Projects fetchDetails)
        {
            var commits = await GetGithubCommits(userID, request.projectID, request.startDate, request.endDate, fetchDetails);
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fetchDetails.Users.GithubAccessToken);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("derinor-app");

            var repo = fetchDetails.ProjectBranches.FirstOrDefault()?.ProjectRepository ?? throw new InvalidOperationException("No repository selected");
            var analysisInputs = new List<GithubCommitAnalysisRequestDTO>();
            var throttler = new SemaphoreSlim(5);

            var tasks = commits.Where(c => c != null).Select(async commit =>
            {
                await throttler.WaitAsync();
                try
                {
                    if (commit?.Sha == null) return null;
                    var detailUrl = $"https://api.github.com/repos/{fetchDetails.Users.GithubUsername}/{repo}/commits/{commit.Sha}";
                    var resp = await client.GetAsync(detailUrl);
                    resp.EnsureSuccessStatusCode();
                    var json = await resp.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;
                    var files = root.TryGetProperty("files", out var filesElement) ? filesElement.EnumerateArray() : Enumerable.Empty<JsonElement>();
                    var patches = files.Select(f => f.TryGetProperty("patch", out var p) ? p.GetString() : null).Where(p => p != null);
                    var message = commit.CommitMessage?.MessageDescription ?? "No commit messages available";
                    return new GithubCommitAnalysisRequestDTO
                    {
                        Sha = commit.Sha,
                        MessageDescription = message,
                        Patch = string.Join("\n\n", patches)
                    };
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    throttler.Release();
                }
            });

            var results = await Task.WhenAll(tasks);
            analysisInputs.AddRange(results.Where(r => r != null && !string.IsNullOrWhiteSpace(r.Patch)));
            return new GeneratedReportDataRequestDTO { AllCommitsData = analysisInputs };
        }

        public async Task<GeminiDataResponseDTO?> GetGeminiData(int userID, GenerateReportRequestDTO request)
        {
            var fetchDetails = await _userRepository.GetFetchingDetails(userID, request.projectID);
            if (fetchDetails == null)
            {
                return null;
            }

            var commitDetails = await GenerateReport(userID, request, fetchDetails);

            if (commitDetails.AllCommitsData == null || !commitDetails.AllCommitsData.Any())
            {
                return new GeminiDataResponseDTO { GeminiMessage = "" };
            }

            var sb = new StringBuilder();
            var developmentPeriod = $"Development Period: {request.startDate:yyyy/MM/dd} – {request.endDate:yyyy/MM/dd}";

            sb.AppendLine("You are an expert changelog reports writer for a startup.");
            sb.AppendLine($"The summary should include Finished Work and {developmentPeriod} at the top, then each notable change (commit message and its code differences) should include a title and description about what was changed that will be understandable to a non-technical person.");
            sb.AppendLine("Please read the following commits and code diffs, then give me a concise summary of notable changes.");
            sb.AppendLine("Structure the report with clear sections like 'New Features' or 'Bug Fixes'. Do not include sensitive information from the code or commits.");
            sb.AppendLine("Here is an example format: Finished Work (Development Period: 2025/09/15 – 2025/09/16)\n\n## New Features\n### Enhanced Search Filters\n**What it does:** Users can now quickly narrow down options by selecting filters like cuisine type, price range, and ratings.\n**Impact:** Makes it easier for users to find exactly what they're in the mood for.");
            sb.AppendLine();

            foreach (var commit in commitDetails.AllCommitsData)
            {
                sb.AppendLine($"Commit SHA: {commit.Sha}");
                sb.AppendLine($"Message: {commit.MessageDescription}");
                sb.AppendLine("Diff:");
                sb.AppendLine("```diff");
                sb.AppendLine(commit.Patch);
                sb.AppendLine("```");
                sb.AppendLine(new string('-', 40));
            }

            var client = _httpClientFactory.CreateClient();
            var apiKey = _configuration["Gemini:ApiKey"];

            client.DefaultRequestHeaders.Add("x-goog-api-key", apiKey);

            var response = await client.PostAsync(
                "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-lite:generateContent",
                new StringContent(
                    JsonSerializer.Serialize(new
                    {
                        contents = new[] {
                    new {
                        parts = new[] {
                            new { text = sb.ToString() }
                        }
                    }
                        }
                    }),
                    Encoding.UTF8,
                    "application/json"));

            response.EnsureSuccessStatusCode();
            var rawJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(rawJson);

            try
            {
                var text = doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
                return new GeminiDataResponseDTO { GeminiMessage = text ?? "No response content from Gemini." };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not parse the expected structure from Gemini response. Details: {ex.Message}");
            }
        }

        public async Task<List<GithubRepositoryResponseDTO>> FetchRepositories(int userID)
        {
            var userData = await _userRepository.GetUsernameByUserID(userID);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userData.GithubAccessToken);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("derinor-app");
            var repos = new List<GithubRepositoryResponseDTO>();
            var page = 1;
            const int maxPages = 10;
            bool hasMore;
            do
            {
                var url = $"https://api.github.com/users/{userData.GithubUsername}/repos?per_page=100&page={page}";
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var pageRepos = JsonSerializer.Deserialize<List<GithubRepositoryResponseDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (pageRepos != null) repos.AddRange(pageRepos);
                response.Headers.TryGetValues("Link", out var linkValues);
                hasMore = linkValues?.FirstOrDefault()?.Contains("rel=\"next\"") == true;
                page++;
            } while (hasMore && page <= maxPages);
            return repos.Select(r => new GithubRepositoryResponseDTO { Id = r.Id, Name = r.Name }).ToList();
        }

        public async Task<List<GithubBranchesResponseDTO>> FetchBranches(int userID, string repositoryName)
        {
            var userData = await _userRepository.GetUsernameByUserID(userID);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userData.GithubAccessToken);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("derinor-app");
            var branches = new List<GithubBranchesResponseDTO>();
            var page = 1;
            const int maxPages = 10;
            bool hasMore;
            do
            {
                var url = $"https://api.github.com/repos/{userData.GithubUsername}/{repositoryName}/branches?per_page=100&page={page}";
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var pageBranches = JsonSerializer.Deserialize<List<GithubBranchesResponseDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (pageBranches != null) branches.AddRange(pageBranches);
                response.Headers.TryGetValues("Link", out var linkValues);
                hasMore = linkValues?.FirstOrDefault()?.Contains("rel=\"next\"") == true;
                page++;
            } while (hasMore && page <= maxPages);
            return branches.Select(b => new GithubBranchesResponseDTO { name = b.name, githubCommitDTO = new GithubCommitDTO { sha = b.githubCommitDTO.sha } }).ToList();
        }

        public async Task PublishProject(PublishProjectDTO publishProjectDTO)
        {
            var publishProject = new ProjectReports
            {
                ProjectID = publishProjectDTO.projectID,
                ProjectReportDescription = publishProjectDTO.reportContent


            };

            await _projectsRepository.PublishProject(publishProject);
        }

        public async Task<List<ProjectReportDTO>> GetReportsByProject(int projectID)
        {
            var projects = await _projectsRepository.GetReportsByProject(projectID);

            var projectDtos = projects.Select(p => new ProjectReportDTO
            {
                projectReportID = p.ProjectID,
                projectReportDescription = p.ProjectReportDescription
            }).ToList();

            return projectDtos;
        }
    }
}