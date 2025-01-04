using Ardalis.Result;
using FluentValidation;
using MediatR;

namespace Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
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
            .SelectMany(r => r.Errors)
            .Where(e => e is not null)
            .Select(e => new ValidationError(e.PropertyName, e.ErrorMessage))
            .ToArray();

        if (errors.Length > 0)
        {
            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var genericArgument = typeof(TResponse).GetGenericArguments()[0];
                var typedResultType = typeof(Result<>).MakeGenericType(genericArgument);

                var invalidMethod = typedResultType.GetMethod(
                    nameof(Result.Invalid),
                    [typeof(IEnumerable<ValidationError>)]
                );

                var typedResult = invalidMethod!.Invoke(null, [errors]);
                return (TResponse)typedResult!;
            }
            else
            {
                return (TResponse)(object)Result.Invalid(errors);
            }
        }

        return await next();
    }
}
