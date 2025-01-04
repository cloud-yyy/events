using Application.Abstractions;
using Application.Dtos;

namespace Application.Categories.CreateCategory;

public record CreateCategoryCommand(string Name) : ICommand<CategoryDto>;
