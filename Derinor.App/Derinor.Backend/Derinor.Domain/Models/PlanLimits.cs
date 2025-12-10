using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.Domain.Models
{
    public static class PlanLimits
    {
        public static int GetMaxProjects(UserPlan plan)
        {
            return plan switch
            {
                UserPlan.Starter => 5,
                UserPlan.Business => 25,
                UserPlan.Enterprise => int.MaxValue,
                _ => 0
            };
        }

        public static int GetMaxReports(UserPlan plan)
        {
            return plan switch
            {
                UserPlan.Starter => 25,
                UserPlan.Business => 100,
                UserPlan.Enterprise => int.MaxValue,
                _ => 0
            };
        }
    }
}
