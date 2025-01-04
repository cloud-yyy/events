using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Categories.CreateCategory;

internal sealed class CreateCategoryCommandHandler(
    ICategoryRepository _categoryRepository,
    IMapper _mapper,
    IUnitOfWork _unitOfWork
) : ICommandHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<Result<CategoryDto>> Handle
        (CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var existed = await _categoryRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existed is not null)
            return Result.Invalid(new ValidationError(
                nameof(request.Name), $"Category with name {request.Name} already exists")
            );

        var category = new Category { Name = request.Name };

        _categoryRepository.Add(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Created(_mapper.Map<CategoryDto>(category));
    }
}
