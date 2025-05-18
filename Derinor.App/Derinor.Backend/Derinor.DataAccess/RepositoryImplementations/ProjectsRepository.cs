﻿using Derinor.DataAccess.RepositoryInterfaces;
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

        public async Task<List<Projects>> AllProjects(string? search)
        {
            var fetchedProjects = _appDbContext.Projects.AsQueryable();

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
    }
}
