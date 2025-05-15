using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.Common.RequestDTOs
{
    public class GithubCommitAnalysisRequestDTO
    {
        public string Sha { get; set; }
        public string MessageDescription { get; set; }
        public string Patch { get; set; }
    }
}
