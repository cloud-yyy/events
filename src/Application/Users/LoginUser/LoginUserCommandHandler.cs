using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using Domain.Repositories;

namespace Application.Users.LoginUser;

internal sealed class LoginUserCommandHandler(
    IUserRepository _userRepository,
    IJwtTokenProvider _jwtTokenProvider,
    IRefreshTokenProvider _refreshTokenProvider,
    IPasswordHasher _passwordHasher
) : ICommandHandler<LoginUserCommand, TokenDto>
{
    public async Task<Result<TokenDto>> Handle
        (LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            return Result.Invalid(
                new ValidationError(nameof(request.Email), "User not found")
            );

        if (!user.PasswordHash.Equals(_passwordHasher.HashPassword(request.Password)))
            return Result.Invalid(
                new ValidationError(nameof(request.Password), "Invalid password")
            );

        var accessToken = _jwtTokenProvider.GenerateToken(user);
        var resfreshToken = await _refreshTokenProvider.GenerateToken(user);

        return new TokenDto(accessToken, resfreshToken);
    }
}
