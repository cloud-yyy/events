using Ardalis.Result;

namespace Application.ErrorResults;

internal static class CategoryResults
{
    public static class NotFound
    {
        public static Result ById(Guid id) => Result.NotFound($"Category with id {id} not found");
        public static Result ByName(string name) => Result.NotFound($"Category with name {name} not found");
    }

    public static class Invalid
    {
        public static Result NameNotUnique(string name)
            => Result.Invalid(new ValidationError(nameof(name), $"Category with name {name} already exists"));

        public static Result HasRelatedEvents(Guid id) 
            => Result.Invalid(new ValidationError(nameof(id), "Cannot delete category with events"));
    }
}
