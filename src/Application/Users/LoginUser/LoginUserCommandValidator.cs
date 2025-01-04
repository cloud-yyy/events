using FluentValidation;

namespace Application.Users.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(e => e.Email).NotEmpty().EmailAddress();

        RuleFor(e => e.Password).NotEmpty().MinimumLength(4).MaximumLength(50);;
    }
}
