using Derinor.DataAccess.RepositoryInterfaces;
using Derinor.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Derinor.DataAccess.RepositoryImplementations
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly AppDbContext _appDbContext;
        public ProjectsRepository(AppDbContext appDbContext) { 
        
            _appDbContext = appDbContext;
        }

        public async Task<List<Projects>> AllProjects(string? search, int userID)
        {
            var fetchedProjects = _appDbContext.Projects
                .Where(p => p.UserID == userID)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                fetchedProjects = fetchedProjects.Where(p => p.ProjectName.Contains(search));
            }

            fetchedProjects = fetchedProjects.OrderByDescending(p => p.ProjectID);
            return await fetchedProjects.ToListAsync();
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
            await _appDbContext.SaveChangesAsync();
        }

        public async Task PublishProject(ProjectReports projectReports)
        {
            await _appDbContext.ProjectReports.AddAsync(projectReports);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<ProjectReports>> GetReportsByProject(int projectID)
        {
            return await _appDbContext.ProjectReports.Where(r => r.ProjectID == projectID).ToListAsync();
        }

    }
}
