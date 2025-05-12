using Derinor.BusinessLogic.ServiceInterfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Derinor.Presentation.Controllers
{

    [ApiController]
    [Route("/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {

            _authService = authService;
            _logger = logger;

        }


        [HttpGet("github-signin")]
        public IActionResult OpenGithub()
        {

            try
            {

                var githubUrl = _authService.OpenGithub();
                return Redirect(githubUrl);

            }
            catch (Exception ex)
            {

                _logger.LogError("Something went wrong opening github");
                return BadRequest("Something went wrong opening github");

            }

        }

        [HttpGet("github-callback")]
        public async Task<IActionResult> GithubCallback([FromQuery] string code)
        {
            try
            {
                var tokenResponse = await _authService.ExchangeCodeForToken(code);

                if (tokenResponse == null)
                {
                    return BadRequest("Something went wrong signing in with github");
                }

                var jwt = await _authService.GetOrCreateUserFromGithubToken(tokenResponse);

                return Ok(new { token = jwt });
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Something went wrong with the github sign in, please try again later");
            }


        }
    }
}
