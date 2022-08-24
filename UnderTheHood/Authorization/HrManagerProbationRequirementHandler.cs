using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace UnderTheHood.Authorization
{
    public class HrManagerProbationRequirementHandler : AuthorizationHandler<HrManagerProbationRequirement> 
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HrManagerProbationRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "EmploymentDate"))
                return Task.CompletedTask;

            var empDate = DateTime.Parse(context.User.FindFirst(x => x.Type == "EmploymentDate").Value);
            var period = DateTime.Now - empDate;

            if (period.Days > (30 * requirement.ProbationMonths))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}