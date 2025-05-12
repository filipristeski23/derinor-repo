using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Derinor.Common.ResponseDTOs
{
    public class GithubUserResponse
    {

        [JsonPropertyName("name")]
        public string FullName { get; set; }

        [JsonPropertyName("id")]
        public int GithubID { get; set; }

    }
}
