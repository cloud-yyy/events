using Application.Abstractions;
using Application.Dtos;

namespace Application.Users.UpdateUser;

public record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    Guid RoleId
) : ICommand<UserDto>;
