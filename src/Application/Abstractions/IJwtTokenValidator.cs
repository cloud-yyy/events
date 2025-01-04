using System.Security.Claims;

namespace Application.Abstractions;

public interface IJwtTokenValidator
{
    public ClaimsPrincipal? ValidateToken(string token);
}
