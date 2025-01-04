using Application.Abstractions;
using Application.Dtos;
using Domain;

namespace Application.Categories.GetAllCategories;

public record GetAllCategoriesQuery(int PageNumber, int PageSize)
    : IQuery<IPagedList<CategoryDto>>;
