using Domain.Entities;

namespace Application.Abstractions;

public interface IRefreshTokenProvider
{
    public Task<string> GenerateToken(User user);
}
