using FluentValidation;

namespace Application.Users.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(t => t.AccessToken).NotEmpty();

        RuleFor(t => t.RefreshToken).NotEmpty();
    }
}
