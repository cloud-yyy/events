using Application.Abstractions;
using Application.Dtos;

namespace Application.Users.LoginUser;

public record LoginUserCommand(string Email, string Password) : ICommand<TokenDto>;
