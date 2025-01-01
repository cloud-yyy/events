using Application.Users.LoginUser;
using FluentValidation;

namespace Application.Users;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(e => e.Email).NotEmpty().EmailAddress();

        RuleFor(e => e.Password).NotEmpty();
    }
}
