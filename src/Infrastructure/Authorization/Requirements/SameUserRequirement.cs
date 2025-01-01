using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Requirements;

public record SameUserRequirement : IAuthorizationRequirement;
