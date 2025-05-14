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

        public async Task<Projects> InsertProject(Projects projectDetails)
        {
            await _appDbContext.Projects.AddAsync(projectDetails);
            await _appDbContext.SaveChangesAsync();
            return projectDetails;
        }

        public async Task InsertBranches(ProjectBranches projectBranches)
        {
            await _appDbContext.ProjectBranches.AddAsync(projectBranches);
        }
    }
}
