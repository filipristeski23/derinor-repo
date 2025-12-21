using Derinor.Application.ServiceInterfaces;
using Derinor.Common.RequestDTOs;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// This endpoint gets all projects for specific user.
        /// </summary>
        [HttpGet("all-projects")]
        [Authorize]
        public async Task<IActionResult> AllProjects([FromQuery] string? search)
        {
            var userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var fetchedProjects = await _projectsService.AllProjects(search, userID);
            return Ok(fetchedProjects);
        }

        /// <summary>
        /// This endpoint creates new project.
        /// </summary>
        [HttpPost("create-project")]
        [Authorize]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDetailsRequestDTO projectDetails)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userID = int.Parse(userIdClaim.Value);

            try
            {
                await _projectsService.CreateProject(projectDetails, userID);
                return Ok("Project Created Successfully");
            }
            catch (Exception ex)
            {
                if (ex.Message == "PLAN_LIMIT_REACHED")
                    return BadRequest("Plan limit reached");

                throw;
            }
        }

        /// <summary>
        /// This endpoint fetches github data for some project and sends it to gemini for processing.
        /// </summary>
        [HttpPost("get-gemini-data")]
        [Authorize]
        public async Task<IActionResult> GetGeminiData([FromBody] GenerateReportRequestDTO request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userID = int.Parse(userIdClaim.Value);

            var projectID = request.projectID;
            var startDate = request.startDate;
            var endDate = request.endDate;

            var serviceRequest = new GenerateReportRequestDTO
            {
                projectID = projectID,
                startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0, DateTimeKind.Utc),
                endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59, 999, DateTimeKind.Utc)
            };

            var generatedReport = await _projectsService.GetGeminiData(userID, serviceRequest);

            return Ok(generatedReport);
        }

        /// <summary>
        /// This endpoint gets all repositories to show for project.
        /// </summary>
        [HttpGet("fetch-repositories")]
        [Authorize]
        public async Task<IActionResult> FetchRepositories()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userID = int.Parse(userIdClaim.Value);

            var fetchedRepositories = await _projectsService.FetchRepositories(userID);
            return Ok(fetchedRepositories);
        }

        /// <summary>
        /// This endpoint gets all branches to show for project.
        /// </summary>
        [HttpGet("fetch-branches")]
        [Authorize]
        public async Task<IActionResult> FetchBranches([FromQuery] string repositoryName)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userID = int.Parse(userIdClaim.Value);

            var fetchedBranches = await _projectsService.FetchBranches(userID, repositoryName);
            return Ok(fetchedBranches);
        }

        /// <summary>
        /// This endpoint publishes the report to the dedicated page.
        /// </summary>
        [HttpPost("publish-report")]
        [Authorize]
        public async Task<IActionResult> PublishProject([FromBody] PublishProjectDTO publishProjectDTO)
        {

            var userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            try
            {
                await _projectsService.PublishProject(publishProjectDTO);
                return Ok("Project Published Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong with publishing project");
            }
        }

        /// <summary>
        /// This endpoint gets all project reports.
        /// </summary>
        [HttpGet("all-by-project/{projectID}")]
        public async Task<IActionResult> GetReportsByProject(int projectID)
        {
            var projects = await _projectsService.GetReportsByProject(projectID);
            return Ok(projects);
        }
    }
}