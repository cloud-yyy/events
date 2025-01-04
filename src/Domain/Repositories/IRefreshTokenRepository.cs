using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Repositories;

public interface IRefreshTokenRepository
{
    public Task<RefreshToken?> GetByUserIdAsync(Guid userId, CancellationToken token = default);
    public Task<RefreshToken?> GetByTokenAsync(string tokenValue, CancellationToken token = default);
    public RefreshToken Add(RefreshToken refreshToken);
    public Task DeleteByConditionAsync(
        Expression<Func<RefreshToken, bool>> condition, 
        CancellationToken token = default
    );
}
