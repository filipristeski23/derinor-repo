using Derinor.Application.ServiceInterfaces;
using Derinor.Common.ResponseDTOs;
using Derinor.DataAccess.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.Application.ServiceImplementations
{
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectsRepository _projectsRepository;
        public ProjectsService(IProjectsRepository projectsRepository) { 
        
            _projectsRepository = projectsRepository;
        }

        public async Task<List<ProjectHomeResponseDTO>> AllProjects()
        {
            var fetchedProjects = await _projectsRepository.AllProjects();

            var fetchedProjectsDTO = fetchedProjects.Select(p => new ProjectHomeResponseDTO
            {
                projectOwner = p.ProjectOwner,
                projectName = p.ProjectName,
                projectDescription = p.ProjectDescription,

            }).ToList();;

            return fetchedProjectsDTO;
        }
    }
}
