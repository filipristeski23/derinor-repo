using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.Common.ResponseDTOs
{
    public class ProjectHomeResponseDTO
    {
        public string projectOwner { get; set; }
        public string projectName { get; set; }
        public string projectDescription { get; set; }
        public int projectID { get; set; }
    }
}
