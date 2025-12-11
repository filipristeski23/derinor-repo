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
            var user = await _userRepository.GetUserById(userID);
            var currentProjects = await _projectsRepository.CountProjects(userID);
            var allowedProjects = PlanLimits.GetMaxProjects(user.Plan);
            if (currentProjects >= allowedProjects)
                throw new Exception("PLAN_LIMIT_REACHED");


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

            Console.WriteLine($"=== STARTING REPORT GENERATION ===");
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
                    Console.WriteLine($"Fetching commit details: {commit.Sha}");
                    var resp = await client.GetAsync(detailUrl);

                    // Check rate limits BEFORE EnsureSuccessStatusCode
                    if (resp.Headers.TryGetValues("X-RateLimit-Remaining", out var remaining))
                    {
                        Console.WriteLine($"GitHub Rate Limit Remaining: {remaining.FirstOrDefault()}");
                    }
                    if (resp.Headers.TryGetValues("X-RateLimit-Reset", out var reset))
                    {
                        Console.WriteLine($"GitHub Rate Limit Reset: {reset.FirstOrDefault()}");
                    }

                    if (resp.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        Console.WriteLine($"!!! HIT GITHUB RATE LIMIT !!!");
                        throw new HttpRequestException($"GitHub API rate limit exceeded. Response: {await resp.Content.ReadAsStringAsync()}");
                    }

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
            Console.WriteLine($"Successfully processed {analysisInputs.Count} commits with patches");
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
            sb.AppendLine("STRICT FORMATTING RULES:");
            sb.AppendLine("1. Do NOT use Markdown symbols like '##', '###', or '**'. use plain text only.");
            sb.AppendLine("2. Use a single dash '-' to list items or separate sections.");
            sb.AppendLine("3. Do NOT mention Commit SHAs, IDs, technical hashes, or filenames in the final output.");
            sb.AppendLine("4. Focus purely on business value and functionality changes.");
            sb.AppendLine($"The summary should include a header line 'Finished Work' followed by '{developmentPeriod}'.");
            sb.AppendLine("Group the changes into logical sections (e.g., New Features, Improvements, Bug Fixes).");
            sb.AppendLine("For each item, provide a simple title and a plain description understandable to a non-technical person.");

            sb.AppendLine("Example Desired Output:");
            sb.AppendLine("Finished Work");
            sb.AppendLine("Development Period: 2025/09/15 – 2025/09/16");
            sb.AppendLine("");
            sb.AppendLine("- New Features");
            sb.AppendLine("  Enhanced Search Filters");
            sb.AppendLine("  Users can now narrow down options by cuisine and price. This makes finding specific items much faster.");
            sb.AppendLine("");
            sb.AppendLine("- Bug Fixes");
            sb.AppendLine("  Login Screen Crash");
            sb.AppendLine("  Fixed an issue where the app would close unexpectedly on the login screen.");
            sb.AppendLine("Analyze the following commits and generate the report based on the rules above:");
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

            Console.WriteLine($"=== GEMINI REQUEST DEBUG ===");
            Console.WriteLine($"About to call Gemini with {commitDetails.AllCommitsData.Count} commits");
            Console.WriteLine($"Total payload characters: {sb.Length}");
            Console.WriteLine($"Estimated tokens: ~{sb.Length / 4}");
            Console.WriteLine($"============================");

            var client = _httpClientFactory.CreateClient();
            var apiKey = _configuration["Gemini:ApiKey"];

            client.DefaultRequestHeaders.Add("x-goog-api-key", apiKey);

            var response = await client.PostAsync(
                "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-lite:generateContent",
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

            Console.WriteLine($"Gemini Response Status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"=== GEMINI ERROR DETAILS ===");
                Console.WriteLine(errorContent);
                Console.WriteLine($"============================");
                throw new HttpRequestException($"Gemini API returned {response.StatusCode}: {errorContent}");
            }

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