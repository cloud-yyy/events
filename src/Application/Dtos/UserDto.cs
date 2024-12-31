namespace Application.Dtos;

public record UserDto(
    Guid Id,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Email,
    RoleDto Role
);
