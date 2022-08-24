using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace UnderTheHood.Authorization
{
    public class HrManagerProbationRequirement : IAuthorizationRequirement
    {
        public HrManagerProbationRequirement(int probationMonths)
        {
            ProbationMonths = probationMonths;
        }
        public int ProbationMonths { get; }
    }
}
