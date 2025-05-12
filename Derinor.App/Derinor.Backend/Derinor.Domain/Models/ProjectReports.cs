using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.Domain.Models
{
    public class ProjectReports
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectReportID { get; set; }
        public string ProjectReportDescription { get; set; }


        public int ProjectID { get; set; }

        [ForeignKey("ProjectID")]
        public Projects Projects { get; set; }

    }
}
