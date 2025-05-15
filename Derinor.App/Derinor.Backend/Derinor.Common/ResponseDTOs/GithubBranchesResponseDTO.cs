using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Derinor.Common.ResponseDTOs
{
    public class GithubBranchesResponseDTO
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("commit")]
        public GithubCommitDTO githubCommitDTO { get; set; }

    }

    public class GithubCommitDTO
    {
        [JsonPropertyName("sha")]
        public string sha { get; set; }
    }
}
