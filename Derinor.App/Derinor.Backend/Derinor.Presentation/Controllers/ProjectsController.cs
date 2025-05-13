using Derinor.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Derinor.Presentation.Controllers
{
    [ApiController]
    [Route("/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _projectsService;
        private readonly ILogger<ProjectsController> _logger;
        public ProjectsController(IProjectsService projectsService)
        {

            _projectsService = projectsService;
        }

        [HttpGet("all-projects")]
        public async Task<IActionResult> AllApartments()
        {
            try
            {
                var fetchedApartments = await _projectsService.AllProjects();
                return Ok(fetchedApartments);
            }
            catch (Exception ex)
            {

                _logger.LogError("Something is wrong with fetchin all projects");
                return BadRequest("Something went wrong with fetching the projects");
            }
        }


    }
}
