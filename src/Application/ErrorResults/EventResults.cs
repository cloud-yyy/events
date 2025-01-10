using Ardalis.Result;

namespace Application.ErrorResults;

internal static class EventResults
{
    public static class NotFound
    {
        public static Result ById(Guid id) => Result.NotFound($"Event with id {id} not found");
        public static Result ByName(string name) => Result.NotFound($"Event with name {name} not found");
    }

    public static class Invalid
    {
        public static Result NameNotUnique(string name)
            => Result.Invalid(new ValidationError(nameof(name), $"Event with name {name} already exists"));
        
        public static Result HasImage(Guid id) => 
            Result.Invalid(new ValidationError(nameof(id), $"Event with id {id} already has image"));

        public static Result HasNoImage(Guid id) => 
            Result.Invalid(new ValidationError(nameof(id), $"Event with id {id} has no image"));

        public static Result HasNoPlaces(Guid id) =>
            Result.Invalid(new ValidationError(nameof(id), $"Event with id {id} has no free places"));
    }
}
