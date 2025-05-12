using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.Domain.Models
{
    public class Projects
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectID { get; set; }

        [StringLength(10)]
        public string ProjectOwner { get; set; }

        [StringLength(50)]
        public string ProjectName { get; set; }

        [StringLength(100)]
        public string ProjectDescription { get; set; }

        public int UserID { get; set; }

        [ForeignKey("UserID")]

        public Users Users { get; set; }
        public ICollection<ProjectBranches> ProjectBranches { get; set; }
        public ICollection<ProjectReports> ProjectReports { get; set; }
    }
}
