namespace Presentation.Requests;

public record RegisterUserRequest(
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Email,
    string Password
);
