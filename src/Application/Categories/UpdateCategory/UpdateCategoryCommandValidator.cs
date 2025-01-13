using FluentValidation;

namespace Application.Categories.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(e => e.Name).NotEmpty().MaximumLength(100);
    }
}
