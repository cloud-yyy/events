using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using Domain.Repositories;

namespace Application.Users.LoginUser;

internal sealed class LoginUserCommandHandler(
    IUserRepository _userRepository,
    IJwtTokenProvider _jwtTokenProvider,
    IRefreshTokenProvider _refreshTokenProvider
) : ICommandHandler<LoginUserCommand, TokenDto>
{
    public async Task<Result<TokenDto>> Handle
        (LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            return Result.NotFound($"User with email {request.Email} not found");

        var accessToken = _jwtTokenProvider.GenerateToken(user);
        var resfreshToken = await _refreshTokenProvider.GenerateJwtToken(user);

        return new TokenDto(accessToken, resfreshToken);
    }
}
