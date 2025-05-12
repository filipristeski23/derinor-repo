using Derinor.Common.ResponseDTOs;
using Derinor.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.BusinessLogic.ServiceInterfaces
{
    public interface IAuthService
    {
        string OpenGithub();

        Task<string> GetOrCreateUserFromGithubToken(GithubTokenResponse githubTokenResponse);

        Task<string> GenerateJwtToken(Users user);

        Task<GithubTokenResponse> ExchangeCodeForToken(string code);

    }
}
