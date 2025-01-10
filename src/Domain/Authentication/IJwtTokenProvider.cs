using Domain.Entities;

namespace Domain.Authentication;

public interface IJwtTokenProvider
{
    public string GenerateToken(User user);
}
