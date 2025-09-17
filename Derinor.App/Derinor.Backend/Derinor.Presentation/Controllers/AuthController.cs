using Derinor.BusinessLogic.ServiceInterfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Derinor.Presentation.Controllers
{

    
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
                var user = new
                {
                    name = "Filip",
                    surname = "Risteski"
                };
                var userData = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(user)));

                return Redirect($"http://localhost:5173/auth/callback?token={jwt}&user={userData}");
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Something went wrong with the github sign in, please try again later"); 
            }


        }
    }
}
