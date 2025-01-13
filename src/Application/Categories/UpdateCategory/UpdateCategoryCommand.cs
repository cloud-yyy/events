using Application.Abstractions;
using Application.Dtos;

namespace Application.Categories.UpdateCategory;

public record UpdateCategoryCommand(Guid Id, string Name)
    : ICommand<CategoryDto>;
