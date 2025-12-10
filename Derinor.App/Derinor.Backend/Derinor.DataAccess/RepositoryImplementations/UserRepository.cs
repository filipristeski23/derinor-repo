using Derinor.DataAccess.RepositoryInterfaces;
using Derinor.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.DataAccess.RepositoryImplementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        public UserRepository(AppDbContext appDbContext)
        {

            _appDbContext = appDbContext;

        }

        public async Task<Users> AddUser(Users user)
        {
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUser(Users user)
        {
            var existingUser = await _appDbContext.Users.FindAsync(user.UserID);
            if (existingUser != null)
            {
                existingUser.FullName = user.FullName;
                existingUser.GithubAccessToken = user.GithubAccessToken;
                existingUser.GithubUsername = user.GithubUsername;
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<Users> GetByGithubID(int githubID)
        {
            var existingGithubUser = await _appDbContext.Users
                .Where(u => u.GithubID == githubID)
                .FirstOrDefaultAsync();
            return existingGithubUser;
        }

        public async Task<Projects> GetFetchingDetails(int userID, int projectID)
        {
            var theDetails = await _appDbContext.Projects
                .Include(p => p.Users)
                .Include(p => p.ProjectBranches)
                .Where(p => p.UserID == userID && p.ProjectID == projectID)
                .FirstOrDefaultAsync();
            return theDetails;
        }

        public async Task<Users> GetUsernameByUserID(int userID)
        {
            var data = await _appDbContext.Users
                .Where(u => u.UserID == userID)
                .FirstOrDefaultAsync();
            return data;
        }

        public async Task<Users> GetUserById(int userID)
        {
            return await _appDbContext.Users
                .Where(u => u.UserID == userID)
                .FirstOrDefaultAsync();
        }

    }
}