using FluentValidation;

namespace Application.Events.GetEventByName;

public class GetEventByNameQueryValidator : AbstractValidator<GetEventByNameQuery>
{
    public GetEventByNameQueryValidator()
    {
        RuleFor(e => e.Name).NotEmpty().MaximumLength(100);
    }
}
