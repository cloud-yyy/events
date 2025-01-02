using System.Security.Claims;
using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using Domain;
using Domain.Repositories;

namespace Application.Users.RefreshToken;

internal sealed class RefreshTokenCommandHandler(
    IRefreshTokenRepository _refreshTokenRepository,
    IJwtTokenProvider _jwtTokenProvider,
    IJwtTokenValidator _jwtTokenValidator,
    IRefreshTokenProvider _refreshTokenProvider,
    IUnitOfWork _unitOfWork
) : ICommandHandler<RefreshTokenCommand, TokenDto>
{
    public async Task<Result<TokenDto>> Handle
        (RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = _jwtTokenValidator.ValidateToken(request.AccessToken)?
            .Claims
            .SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim is null)
            return Result.Invalid(
                new ValidationError(nameof(request.RefreshToken), "Invalid access token")
            );

        var userId = Guid.Parse(userIdClaim);
        var refreshToken = await _refreshTokenRepository.GetByUserIdAsync(userId, cancellationToken);
        
        if (refreshToken is not null && 
            refreshToken.Expires > DateTime.UtcNow &&
            refreshToken.Token.Equals(request.RefreshToken))
        {
            var newAccessToken = _jwtTokenProvider.GenerateToken(refreshToken.User!);
            var newResfreshToken = await _refreshTokenProvider.GenerateJwtToken(refreshToken.User!);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new TokenDto(newAccessToken, newResfreshToken);
        }

        return Result.Invalid(
            new ValidationError(nameof(request.RefreshToken), "Invalid refresh token")
        );
    }
}
