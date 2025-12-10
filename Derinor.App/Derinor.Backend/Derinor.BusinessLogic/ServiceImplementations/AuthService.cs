using Derinor.BusinessLogic.ServiceInterfaces;
using Derinor.Common.ResponseDTOs;
using Derinor.DataAccess.RepositoryInterfaces;
using Derinor.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Derinor.BusinessLogic.ServiceImplementations
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserRepository _userRepository;
        public AuthService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IUserRepository userRepository)
        {

            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _userRepository = userRepository;
        }

        public string OpenGithub(string state)
        {
            var clientID = _configuration["GithubOAuth:ClientId"];
            var redirectUri = _configuration["GithubOAuth:RedirectUri"];
            var scope = "read:user user:email repo repo:status repo_deployment";
            return $"https://github.com/login/oauth/authorize?client_id={clientID}&redirect_uri={redirectUri}&scope={scope}&state={state}&prompt=select_account";
        }

        public async Task<GithubTokenResponse> ExchangeCodeForToken(string code)
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token");

            var paremeters = new Dictionary<string, string>
            {
                {"code", code },
                {"client_id", _configuration["GithubOAuth:ClientId"] },
                {"client_secret",_configuration["GithubOAuth:ClientSecret"]  },
                {"redirect_uri", _configuration["GithubOAuth:RedirectUri"] }
            };

            request.Content = new FormUrlEncodedContent(paremeters);
            request.Headers.Add("Accept", "application/json");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<GithubTokenResponse>(content);
            return tokenResponse;
        }

        public async Task<string> GetOrCreateUserFromGithubToken(GithubTokenResponse githubTokenResponse, string state)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", githubTokenResponse.AccessToken);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("derinor-app");

            var response = await client.GetAsync("https://api.github.com/user");

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var githubUser = JsonSerializer.Deserialize<GithubUserResponse>(content);

            var githubID = githubUser.GithubID;
            var fullName = githubUser.FullName ?? githubUser.Login;
            var githubUsername = githubUser.Login;
            var accessToken = githubTokenResponse.AccessToken;

            var user = await _userRepository.GetByGithubID(githubID);

            if (user == null)
            {
                if (string.IsNullOrEmpty(state))
                    return "NO_PLAN";

                var newUser = new Users
                {
                    GithubID = githubID,
                    FullName = fullName,
                    GithubAccessToken = accessToken,
                    GithubUsername = githubUsername,
                    Plan = Enum.Parse<UserPlan>(state),
                };

                user = await _userRepository.AddUser(newUser);
            }
            else
            {
                user.FullName = fullName;
                user.GithubAccessToken = accessToken;
                user.GithubUsername = githubUsername;

                if (!string.IsNullOrEmpty(state))
                    user.Plan = Enum.Parse<UserPlan>(state);

                await _userRepository.UpdateUser(user);

               
                if (!Enum.IsDefined(typeof(UserPlan), user.Plan) || user.Plan == UserPlan.NoPlan)
                    return "NO_PLAN";
            }

            return await GenerateJwtToken(user);
        }

        public async Task<string> GenerateJwtToken(Users user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim("githubID", user.GithubID.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}