using Application.Abstractions;
using Ardalis.Result;
using Domain;
using Domain.Repositories;

namespace Application.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler(
    ICategoryRepository _categoryRepository,
    IUnitOfWork _unitOfWork
) : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
            return Result.NotFound($"Category with id {request.Id} not found");

        if (category.Events.Count > 0)
            return Result.Invalid(new ValidationError(nameof(request.Id), "Cannot delete category with events"));

        _categoryRepository.Delete(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.NoContent();
    }
}
