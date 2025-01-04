using System.Security.Cryptography;
using Application.Abstractions;
using Domain;
using Domain.Entities;
using Domain.Repositories;
using Application.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Authentication;

public class RefreshTokenProvider(
    IRefreshTokenRepository _refreshTokenRepository,
    IUnitOfWork _unitOfWork,
    IOptions<RefreshTokenOptions> _options
) : IRefreshTokenProvider
{
    public async Task<string> GenerateToken(User user)
    {
        await _refreshTokenRepository
            .DeleteByConditionAsync(token => token.UserId == user.Id);

        var token = new RefreshToken
        {
            UserId = user.Id,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
            Expires = DateTime.UtcNow.Add(_options.Value.Lifetime),
            User = user
        };

        _refreshTokenRepository.Add(token);
        await _unitOfWork.SaveChangesAsync();

        return token.Token;
    }
}
