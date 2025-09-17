using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.Common.RequestDTOs
{
    public class GenerateReportRequestDTO
    {
        public int projectID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
