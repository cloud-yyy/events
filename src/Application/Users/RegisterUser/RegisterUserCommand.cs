using Application.Abstractions;
using Application.Dtos;

namespace Application.Users.RegisterUser;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : ICommand<UserDto>;
