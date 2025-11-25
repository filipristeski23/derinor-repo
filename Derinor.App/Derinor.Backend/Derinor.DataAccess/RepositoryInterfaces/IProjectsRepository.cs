using Derinor.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.DataAccess.RepositoryInterfaces
{
    public interface IProjectsRepository
    {
        Task<List<Projects>> AllProjects(string? search, int userID);

        Task<Projects> InsertProject(Projects projectDetails);

        Task InsertBranches(ProjectBranches projectBranches);

        Task PublishProject(ProjectReports projectReports);

        Task<List<ProjectReports>> GetReportsByProject(int projectID);
    }
}
