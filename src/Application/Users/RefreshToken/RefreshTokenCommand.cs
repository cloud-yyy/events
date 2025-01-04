using Application.Abstractions;
using Application.Dtos;

namespace Application.Users.RefreshToken;

public record RefreshTokenCommand(string AccessToken, string RefreshToken)
    : ICommand<TokenDto>;
