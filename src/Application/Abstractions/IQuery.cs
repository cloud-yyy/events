using Ardalis.Result;
using MediatR;

namespace Application.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
