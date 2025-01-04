using Application.Abstractions;

namespace Application.Categories.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : ICommand;
