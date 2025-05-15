using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.Domain.Models
{
    [Table("Users")]
    public class Users
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [StringLength(50)]
        public string? FullName { get; set; }
        public int GithubID { get; set; }
        public string GithubUsername { get; set; }
        public string GithubAccessToken { get; set; }

        public ICollection<Projects> Projects { get; set; }

        

    }
}
