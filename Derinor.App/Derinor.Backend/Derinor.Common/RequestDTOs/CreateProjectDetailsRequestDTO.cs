using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.Common.RequestDTOs
{
    public class CreateProjectDetailsRequestDTO
    {
        public string projectOwner {  get; set; }

        public string projectName { get; set; } 

        public string projectDescription { get; set; }

        public ProjectBranchesDTO projectBranches { get; set; }

        public class ProjectBranchesDTO {
            public string projectRepository { get; set; }
            public string projectProductionBranch { get; set; }
        }
        
    }
}
