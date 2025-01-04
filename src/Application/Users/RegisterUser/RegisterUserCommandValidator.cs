using FluentValidation;

namespace Application.Users.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(e => e.Email).NotEmpty().EmailAddress();

        RuleFor(e => e.Password).NotEmpty().MinimumLength(4).MaximumLength(50);

        RuleFor(e => e.FirstName).NotEmpty().MaximumLength(50);

        RuleFor(e => e.LastName).NotEmpty().MaximumLength(50);
    }
}
