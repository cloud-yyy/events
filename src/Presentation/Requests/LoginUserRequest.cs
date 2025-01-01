using Application.Abstractions;
using Application.Dtos;

namespace Presentation.Requests;

public record LoginUserRequest(string Email, string Password) : ICommand<TokenDto>;
