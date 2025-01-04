using FluentValidation;

namespace Application.Users.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(e => e.FirstName).NotEmpty().MaximumLength(50);

        RuleFor(e => e.LastName).NotEmpty().MaximumLength(50);
    }
}
