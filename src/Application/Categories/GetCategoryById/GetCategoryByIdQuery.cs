using Application.Abstractions;
using Application.Dtos;

namespace Application.Categories.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryDto>;
