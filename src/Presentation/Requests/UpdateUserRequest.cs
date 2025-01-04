using Application.Dtos;

namespace Presentation.Requests;

public record UpdateUserRequest(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    RoleDto Role
);
