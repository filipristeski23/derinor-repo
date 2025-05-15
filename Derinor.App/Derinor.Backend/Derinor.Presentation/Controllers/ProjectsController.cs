using Derinor.Application.ServiceInterfaces;
using Derinor.Common.RequestDTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Derinor.Presentation.Controllers
{
    [ApiController]
    [Route("/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _projectsService;
        private readonly ILogger<ProjectsController> _logger;
        public ProjectsController(IProjectsService projectsService, ILogger<ProjectsController> logger)
        {

            _projectsService = projectsService;
            _logger = logger;
        }

        [HttpGet("all-projects")]
        public async Task<IActionResult> AllProjects([FromQuery] string? searchData)
        {
            try
            {
                var fetchedApartments = await _projectsService.AllProjects(searchData);
                return Ok(fetchedApartments);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Something is wrong with fetchin all projects");
                return BadRequest("Something went wrong with fetching the projects");
            }
        }

        [HttpPost("create-project")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDetailsRequestDTO projectDetails)
        {
            try
            {
                var userID = 1;

                await _projectsService.CreateProject(projectDetails, userID);
                return Ok("Project Created Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something is wrong with the creation of a new project");
                return BadRequest("Something went wrong with the creation of a new project");
            }

        }

        [HttpGet("get-gemini-data")]
        public async Task<IActionResult> GetGeminiData(int projectID)
        {
            try
            {

                var userIDString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIDString, out int userID))
                {
                    _logger.LogWarning("Invalid user ID format from frontend: {UserID}", userIDString);
                    return Unauthorized("Invalid Data");
                }

                var generatedReport = await _projectsService.GenerateReport(userID, projectID);
                return Ok(generatedReport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something is wrong with the generation of a report");
                return BadRequest("Something went wrong with the generation of a report");
            }
        }

        [HttpGet("fetch-repositories")]
        public async Task<IActionResult> FetchRepositories()
        {
            try
            {
                var userID = 1;

                var fetchedRepositories = await _projectsService.FetchRepositories(userID);
                return Ok(fetchedRepositories);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Something is wrong with the fetching of repositories");
                return BadRequest("Something is wrong with the fetching of repositories");

            }

        }

        [HttpGet("fetch-branches")]
        public async Task<IActionResult> FetchBranches([FromQuery] string repositoryName)
        {
            try
            {
                var userID = 1;

                var fetchedBranches = await _projectsService.FetchBranches(userID, repositoryName);
                return Ok(fetchedBranches);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Something is wrong with the fetching of branches");
                return BadRequest("Something is wrong with the fetching of branches");

            }

        }
    }
}
