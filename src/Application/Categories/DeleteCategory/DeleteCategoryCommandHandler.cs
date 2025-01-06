using Application.Abstractions;
using Application.ErrorResults;
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
            return CategoryResults.NotFound.ById(request.Id);

        if (category.Events.Count > 0)
            return CategoryResults.Invalid.HasRelatedEvents(request.Id);

        _categoryRepository.Delete(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.NoContent();
    }
}
