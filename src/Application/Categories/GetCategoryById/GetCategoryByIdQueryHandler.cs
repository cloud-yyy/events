using Application.Abstractions;
using Application.Dtos;
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
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category is null)
            return Result.NotFound($"Category with id {request.Id} not found");

        return Result.Success(_mapper.Map<CategoryDto>(category));
    }
}
