using System.Security.Claims;

namespace Domain.Authentication;

public interface IJwtTokenValidator
{
    public ClaimsPrincipal? ValidateToken(string token);
}
