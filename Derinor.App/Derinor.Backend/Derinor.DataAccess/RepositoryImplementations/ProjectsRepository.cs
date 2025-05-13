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
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly AppDbContext _appDbContext;
        public ProjectsRepository(AppDbContext appDbContext) { 
        
            _appDbContext = appDbContext;
        }

        public async Task<List<Projects>> AllProjects()
        {
            var fetchedProjects = await _appDbContext.Projects.ToListAsync();
            return fetchedProjects;
        }
    }
}
