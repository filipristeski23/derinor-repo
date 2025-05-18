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
    using System.Text.Json.Serialization;
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

            public async Task<List<ProjectHomeResponseDTO>> AllProjects(string? search)
            {
                var fetchedProjects = await _projectsRepository.AllProjects(search);

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
                    StartingDate = DateTime.UtcNow
                });
            }

            public async Task<List<GithubCommitResponseDTO>> GetGithubCommits(int userID, int projectID)
            {
                var details = await _userRepository.GetFetchingDetails(userID, projectID);
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
                var since = branch.StartingDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                var until = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

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

                        if (pageCommits?.Count == 0 || pageCommits == null)
                        {
                            hasMore = false;
                            break;
                        }

                        if (commits.Any() && commits.Last().Sha == pageCommits.First().Sha)
                        {

                            break;
                        }

                        if (pageCommits?.Count == 0) break;

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

            public async Task<GeneratedReportDataRequestDTO> GenerateReport(int userID, int projectID)
            {
                var commits = await GetGithubCommits(userID, projectID);
                var fetchDetails = await _userRepository.GetFetchingDetails(userID, projectID);
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
                        
                        if (commit?.Sha == null)
                        {
                            return null;
                        }

                        var detailUrl = $"https://api.github.com/repos/{fetchDetails.Users.GithubUsername}/{repo}/commits/{commit.Sha}";
                        var resp = await client.GetAsync(detailUrl);
                        resp.EnsureSuccessStatusCode();

                        var json = await resp.Content.ReadAsStringAsync();
                        using var doc = JsonDocument.Parse(json);
                        var root = doc.RootElement;

                       
                        var files = root.TryGetProperty("files", out var filesElement) ? filesElement.EnumerateArray() : Enumerable.Empty<JsonElement>();

                        var patches = files.Select(f => {
                                if (f.TryGetProperty("patch", out var patchElement))
                                    return patchElement.GetString();
                                return null;
                        }).Where(p => !string.IsNullOrEmpty(p));

                        
                        var message = commit.CommitMessage?.MessageDescription ?? "No commit messages available";

                        return new GithubCommitAnalysisRequestDTO
                        {
                            Sha = commit.Sha,
                            MessageDescription = message,
                            Patch = string.Join("\n\n", patches)
                        };

                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                    finally
                    {
                        throttler.Release();
                    }
                });

                var results = await Task.WhenAll(tasks);
                analysisInputs.AddRange(results.Where(r => r != null));

                return new GeneratedReportDataRequestDTO { AllCommitsData = analysisInputs };
            }

            public async Task<GeminiDataResponseDTO> GetGeminiData(int userID, int projectID)
            {
                var commitDetails = await GenerateReport(userID, projectID);
                var sb = new StringBuilder();

                sb.AppendLine("You are an expert changelog reports writer for a startup.");
                sb.AppendLine("Read the following commits and code diffs,");
                sb.AppendLine("then give me a concise summary of notable changes made based on the commit messages and code changes");
                sb.AppendLine("The summary should include Finished Work and development period at the top,than each notable change (commit message and its code differences) include title and description about what was changed that will be understandable to non technical person");
                sb.AppendLine("It should be structured and formatted in understandable way, you can include things like features developed, bug fixes, make sure to not include sensitive informations from the code or commits");
                sb.AppendLine("Here is an example of how it should look like - Finished Work (Development Period: 03/09 – 03/16)\r\n\r\nNew Features\r\nEnhanced Restaurant Search Filters\r\nWhat it does: Users can now quickly narrow down restaurant options by selecting filters like cuisine type, price range, and customer ratings.\r\nImpact: Makes it easier for users to find exactly what they're in the mood for.\r\nTechnical Note: We updated our search algorithms and improved the data indexing process to ensure the filters work faster and more accurately.\r\nCustomer Photo Reviews\r\nWhat it does: Customers can attach photos to their reviews, offering a more\r\n");
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

                var response = await client.PostAsync(
                    $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}",
                    new StringContent(
                        JsonSerializer.Serialize(new
                        {
                            contents = new[]
                            {
                              new
                              {
                                parts = new[]
                                {
                                  new { text = sb.ToString() }
                                }
                              }
                            }
                        }),
                        Encoding.UTF8,
                        "application/json"
                    )
                );

                response.EnsureSuccessStatusCode();

                var rawJson = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(rawJson);

                try
                {
                    var candidates = doc.RootElement.GetProperty("candidates");
                    if (candidates.GetArrayLength() == 0) throw new InvalidOperationException("No candidates in response");

                    var firstCandidate = candidates[0];
                    var content = firstCandidate.GetProperty("content");
                    var parts = content.GetProperty("parts");

                    if (parts.GetArrayLength() == 0) throw new InvalidOperationException("No content parts in response");

                    var reply = parts[0].GetProperty("text").GetString() ?? throw new InvalidOperationException("Empty response from Gemini");

                    return new GeminiDataResponseDTO { GeminiMessage = reply };
                }
                catch (KeyNotFoundException ex)
                {
                    throw new InvalidOperationException($"Missing expected property in Gemini response: {ex.Message}");
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
                var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                do
                {
                    var url = $"https://api.github.com/users/{userData.GithubUsername}/repos?per_page=100&page={page}";
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var pageRepos = JsonSerializer.Deserialize<List<GithubRepositoryResponseDTO>>(
                        await response.Content.ReadAsStringAsync(),
                        jsonOptions
                    );

                    repos.AddRange(pageRepos);

                    string linkHeader = null;

                    if (response.Headers.TryGetValues("Link", out var values))
                        linkHeader = values.FirstOrDefault();

                    hasMore = linkHeader?.Contains("rel=\"next\"") == true && page < maxPages;

                    hasMore = linkHeader?.Contains("rel=\"next\"") == true && page <= maxPages;
                    page++;

                } while (hasMore);

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
                var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                do
                {
                    var url = $"https://api.github.com/repos/{userData.GithubUsername}/{repositoryName}/branches?per_page=100&page={page}";
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var pageBranches = JsonSerializer.Deserialize<List<GithubBranchesResponseDTO>>(
                        await response.Content.ReadAsStringAsync(),
                        jsonOptions
                    );

                    branches.AddRange(pageBranches);

                    string linkHeader = null;

                    if (response.Headers.TryGetValues("Link", out var values))
                    linkHeader = values.FirstOrDefault();

                    hasMore = linkHeader?.Contains("rel=\"next\"") == true && page < maxPages;

                    hasMore = linkHeader?.Contains("rel=\"next\"") == true && page <= maxPages;

                    page++;

                } 
                
                while (hasMore);

                return branches.Select(b => new GithubBranchesResponseDTO
                {
                    name = b.name,
                    githubCommitDTO = new GithubCommitDTO { sha = b.githubCommitDTO.sha }

                }).ToList();
            }
        }
    }