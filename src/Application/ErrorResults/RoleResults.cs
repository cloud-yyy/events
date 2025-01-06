using Ardalis.Result;

namespace Application.ErrorResults;

internal static class RoleResults
{
    public static class NotFound
    {
        public static Result ById(Guid id) => Result.NotFound($"Role with id {id} not found");
        public static Result ByName(string name) => Result.NotFound($"Role with name {name} not found");
    }

    public static class Invalid
    {
        public static Result NameNotUnique(string name) =>
            Result.Invalid(new ValidationError(nameof(name), $"Role with name {name} already exists"));
    }
}
