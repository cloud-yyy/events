using System.Linq.Expressions;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class RefreshTokenRepository(
    ApplicationDbContext _context
) : IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByTokenAsync(string tokenValue, CancellationToken token = default)
    {
        return await _context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == tokenValue, token);
    }

    public async Task<RefreshToken?> GetByUserIdAsync(Guid userId, CancellationToken token = default)
    {
        return await _context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.UserId == userId, token);
    }

    public RefreshToken Add(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        return refreshToken;
    }

    public async Task DeleteByConditionAsync(
        Expression<Func<RefreshToken, bool>> condition, 
        CancellationToken token = default)
    {
        await _context.RefreshTokens
            .Where(condition)
            .ExecuteDeleteAsync(token);
    }
}
