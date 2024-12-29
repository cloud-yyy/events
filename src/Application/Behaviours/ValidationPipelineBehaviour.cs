using Ardalis.Result;
using FluentValidation;
using MediatR;

namespace Application.Behaviours;

public class ValidationPipelineBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var errors = _validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(vf => vf is not null)
            .Select(vf => new ValidationError(vf.PropertyName, vf.ErrorMessage))
            .ToArray();

        if (errors.Length > 0)
            return (TResponse)Result.Invalid(errors);

        return await next();
    }
}
