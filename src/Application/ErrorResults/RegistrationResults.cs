using Ardalis.Result;

namespace Application.ErrorResults;

internal static class RegistrationResults
{
    public static class NotFound
    {
        public static Result ByUserIdAndEventId(Guid userId, Guid eventId) =>
            Result.NotFound($"Registration for user {userId} to event {eventId} not found");
    }

    public static class Invalid
    {
        public static Result AlreadyRegistered(Guid userId, Guid eventId) =>
            Result.Invalid(new ValidationError($"User {userId} is already registered to event {eventId}"));
    }
}
