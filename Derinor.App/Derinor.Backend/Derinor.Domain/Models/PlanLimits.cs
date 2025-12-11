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
                UserPlan.Business => 6,
                UserPlan.Enterprise => int.MaxValue,
                _ => 0
            };
        }
    }
}
