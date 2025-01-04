using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using AutoMapper;
using Domain;
using Domain.Repositories;

namespace Application.Categories.GetAllCategories;

public class GetAllCategoriesQueryHandler(
    ICategoryRepository _categoryRepository,
    IMapper _mapper
) : IQueryHandler<GetAllCategoriesQuery, IPagedList<CategoryDto>>
{
    public async Task<Result<IPagedList<CategoryDto>>> Handle
        (GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var pagedEntities = await _categoryRepository
            .GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);

        return Result.Success(pagedEntities.ConvertTo(c => _mapper.Map<CategoryDto>(c)));
    }
}
