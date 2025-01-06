using Application.Abstractions;
using Application.Dtos;
using Application.ErrorResults;
using Ardalis.Result;
using AutoMapper;
using Domain.Repositories;

namespace Application.Categories.GetCategoryById;

public class GetCategoryByIdQueryHandler(
    ICategoryRepository _categoryRepository,
    IMapper _mapper
) : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
{
    public async Task<Result<CategoryDto>> Handle
        (GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category is null)
            return CategoryResults.NotFound.ById(request.Id);

        return Result.Success(_mapper.Map<CategoryDto>(category));
    }
}
