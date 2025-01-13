using FluentValidation;

namespace Application.Categories.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(e => e.Name).NotEmpty().MaximumLength(100);
    }
}
