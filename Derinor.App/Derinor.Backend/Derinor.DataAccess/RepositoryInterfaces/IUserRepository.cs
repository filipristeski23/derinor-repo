using Derinor.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.DataAccess.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task AddUser(Users user);
        Task UpdateUser(Users user);
        Task<Users> GetByGithubID(int githubID);
        Task<Projects> GetFetchingDetails(int userID, int projectID);

        Task<Users> GetUsernameByUserID(int userID);
    }
}
