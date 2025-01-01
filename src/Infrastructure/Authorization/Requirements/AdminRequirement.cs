using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Requirements;

public record AdminRequirement : IAuthorizationRequirement
{
    public const string RoleName = "Admin";
}
