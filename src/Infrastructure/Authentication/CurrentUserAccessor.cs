using System.Security.Claims;
using Domain.Authentication;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication;

public class CurrentUserAccessor : ICurrentUserAccessor
{
    private readonly Guid? _userId;
    private readonly string? _role;

    public Guid? UserId => _userId;

    public string? Role => _role;

    public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        var claims = httpContextAccessor.HttpContext?.User.Claims;

        var userIdStr = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        _userId = userIdStr is not null ? Guid.Parse(userIdStr) : null;

        _role = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
    }
}
