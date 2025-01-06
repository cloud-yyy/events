using Ardalis.Result;

namespace Application.ErrorResults;

internal static class UserResults
{
    public static class NotFound
    {
        public static Result ById(Guid id) => Result.NotFound($"User with id {id} not found");
        public static Result ByEmail(string email) => Result.NotFound($"User with email {email} not found");
    }

    public static class Invalid
    {
        public static Result InvalidPassword() => 
            Result.Invalid(new ValidationError("Invalid password"));

        public static Result InvalidAccessToken() => 
            Result.Invalid(new ValidationError("Invalid access token"));

        public static Result InvalidRefreshToken() => 
            Result.Invalid(new ValidationError("Invalid refresh token"));

        public static Result EmailNotUnique(string email) => 
            Result.Invalid(new ValidationError(nameof(email), $"User with email {email} already exists"));

        public static Result CannotUpdate(Guid id) =>
            Result.Invalid(new ValidationError(nameof(id), $"You are not allowed to update user with id {id}"));

        public static Result CannotGrantRole(string roleName) =>
            Result.Invalid(new ValidationError(nameof(roleName), $"You are not allowed to grant {roleName} role"));
    }
}
