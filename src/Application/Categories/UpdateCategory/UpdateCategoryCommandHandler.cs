using Application.Abstractions;
using Application.Dtos;
using Application.ErrorResults;
using Ardalis.Result;
using AutoMapper;
using Domain.Repositories;

namespace Application.Categories.UpdateCategory;

public class UpdateCategoryCommandHandler(
    ICategoryRepository _categoryRepository,
    IUnitOfWork _unitOfWork,
    IMapper _mapper
) : ICommandHandler<UpdateCategoryCommand, CategoryDto>
{
    public async Task<Result<CategoryDto>> Handle
        (UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category is null)
            return CategoryResults.NotFound.ById(request.Id);

        var existed = await _categoryRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existed is not null && existed.Id != category.Id)
            return CategoryResults.Invalid.NameNotUnique(request.Name);

        category.Name = request.Name;
        _categoryRepository.Update(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(_mapper.Map<CategoryDto>(category));
    }
}
