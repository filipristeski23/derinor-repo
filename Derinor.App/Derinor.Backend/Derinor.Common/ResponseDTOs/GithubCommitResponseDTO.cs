using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.Common.ResponseDTOs
{
    public class GithubCommitResponseDTO
    {
        public string Sha {  get; set; }
        public CommitMessage CommitMessage { get; set; }
    }

    public class CommitMessage
    {
        public string MessageDescription { get; set; }
    }
}

