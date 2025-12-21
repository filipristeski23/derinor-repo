using Derinor.BusinessLogic.ServiceInterfaces;
using Derinor.Common.RequestDTOs;
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

        /// <summary>
        /// This endpoint is used to access github to login or signup
        /// </summary>
        [HttpGet("github-signin")]
        public IActionResult OpenGithub()
        {

            try
            {

                var githubUrl = _authService.OpenGithub(null);
                return Redirect(githubUrl);

            }
            catch (Exception ex)
            {

                _logger.LogError("Something went wrong opening github");
                return BadRequest("Something went wrong opening github");

            }

        }

        /// <summary>
        /// This endpoint is used for github to callback our app after authorization.
        /// </summary>
        [HttpGet("github-callback")]
        public async Task<IActionResult> GithubCallback([FromQuery] string code, [FromQuery] string state)
        {
            try
            {
                var tokenResponse = await _authService.ExchangeCodeForToken(code);

                if (tokenResponse == null)
                {
                    return BadRequest("Something went wrong signing in with github");
                }

                var jwt = await _authService.GetOrCreateUserFromGithubToken(tokenResponse, state);

                if (jwt == "NO_PLAN")
                    return BadRequest("Please go to the pricing page and choose a plan.");

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

        /// <summary>
        /// This endpoint is used to change the plan for a user.
        /// </summary>
        [HttpGet("select-plan")]
        public IActionResult SelectPlan([FromQuery] string plan)
        {
            var githubUrl = _authService.OpenGithub(plan);
            return Redirect(githubUrl);
        }

    }
}
