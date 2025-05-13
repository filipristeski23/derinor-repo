using Derinor.Common.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.Application.ServiceInterfaces
{
    public interface IProjectsService
    {
        Task<List<ProjectHomeResponseDTO>> AllProjects();
    }
}
