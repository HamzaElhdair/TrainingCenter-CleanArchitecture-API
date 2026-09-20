using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TrainingCenter_Core.Interfaces;

namespace TrainingCenter_API.Authorization
{
    public class SameUserOrAdminAuthorizationHandler : AuthorizationHandler<SameUserOrAdminRequirement, IOwnedResource>
    {
        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SameUserOrAdminRequirement requirement,
        IOwnedResource resource)
        {
            var userRole = context.User.FindFirstValue(ClaimTypes.Role);


            if (userRole == "Admin" || userRole == "SuperAdmin")
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }


            var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out int currentUserId))
            {

                if (resource.UserId.HasValue && resource.UserId.Value == currentUserId)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
