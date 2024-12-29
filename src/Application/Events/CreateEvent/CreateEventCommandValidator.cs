using FluentValidation;

namespace Application.Events.CreateEvent;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(e => e.Name).NotEmpty().MaximumLength(100);

        RuleFor(e => e.Description).NotEmpty().MaximumLength(500);

        RuleFor(e => e.Place).NotEmpty().MaximumLength(100);

        RuleFor(e => e.Category).NotEmpty().MaximumLength(40);

        RuleFor(e => e.MaxParticipants).GreaterThan(0);
    }
}
