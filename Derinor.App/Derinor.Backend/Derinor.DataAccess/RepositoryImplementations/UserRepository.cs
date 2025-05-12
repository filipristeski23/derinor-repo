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
        public UserRepository(AppDbContext appDbContext) { 
        
            _appDbContext = appDbContext;

        }

        public async Task AddUser(Users user)
        {
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateUser(Users user)
        {
            var existingUser = await _appDbContext.Users.FindAsync(user.GithubID);
            existingUser.FullName = user.FullName;
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Users> GetByGithubID(int githubID)
        {
            var existingGithubUser = await _appDbContext.Users.FindAsync(githubID);
            return existingGithubUser;
        }
    }
}
