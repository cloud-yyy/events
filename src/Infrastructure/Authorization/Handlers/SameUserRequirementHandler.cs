using System.Security.Claims;
using Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Handlers;

public class SameUserRequirementHandler : AuthorizationHandler<SameUserRequirement, Guid>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, SameUserRequirement requirement, Guid resource)
    {
        var idStr = context.User.Claims
            .First(c => c.Type == ClaimTypes.NameIdentifier)?
            .Value;

        if (idStr is not null && Guid.Parse(idStr) == resource)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
