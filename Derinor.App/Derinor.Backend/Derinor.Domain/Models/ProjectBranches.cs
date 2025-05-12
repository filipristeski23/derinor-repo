using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.Domain.Models
{
    public class ProjectBranches
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectBranchesID { get; set; }
        public string ProjectRepository {  get; set; }
        public string ProjectProductionBranch { get; set; }


        public int ProjectID { get; set; }

        [ForeignKey("ProjectID")]
        public Projects Projects { get; set; }




    }
}
