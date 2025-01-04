using System.Security.Claims;
using Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Handlers;

public class AdminRequirementHandler : AuthorizationHandler<AdminRequirement>
{
    protected override Task HandleRequirementAsync
        (AuthorizationHandlerContext context, AdminRequirement requirement)
    {
        var roleClaim = context.User.FindFirst(c => c.Type == ClaimTypes.Role);

        if (roleClaim is not null && roleClaim.Value == AdminRequirement.RoleName)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
