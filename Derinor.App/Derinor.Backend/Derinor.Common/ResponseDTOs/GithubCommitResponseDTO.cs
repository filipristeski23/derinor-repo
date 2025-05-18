using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Derinor.Common.ResponseDTOs
{

    public class GithubCommitResponseDTO
    {
        [JsonPropertyName("sha")]
        public string Sha { get; set; }

        
        [JsonPropertyName("commit")]
        public CommitMessageDto CommitMessage { get; set; }
    }

    public class CommitMessageDto
    {
       
        [JsonPropertyName("message")]
        public string MessageDescription { get; set; }
    }
}

