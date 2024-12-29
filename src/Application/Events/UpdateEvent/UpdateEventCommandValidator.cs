using FluentValidation;

namespace Application.Events.UpdateEvent;

public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(e => e.Name).NotEmpty().MaximumLength(100);

        RuleFor(e => e.Description).NotEmpty().MaximumLength(500);

        RuleFor(e => e.Place).NotEmpty().MaximumLength(100);

        RuleFor(e => e.Category).NotEmpty().MaximumLength(40);

        RuleFor(e => e.MaxParticipants).GreaterThan(0);
    }
}
