using Domain.Entities;

namespace Domain.Authentication;

public interface IRefreshTokenProvider
{
    public Task<string> GenerateToken(User user);
}
