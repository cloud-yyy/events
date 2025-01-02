using Application.Abstractions;
using Ardalis.Result;
using Domain;
using Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Application.Registrations.DeleteRegistration;

internal sealed class DeleteRegistrationCommandHandler(
    IRegistrationRepository _registrationRepository,
    IUnitOfWork _unitOfWork,
    IAuthorizationService _authorizationService,
    IHttpContextAccessor _httpContextAccessor
) : ICommandHandler<DeleteRegistrationCommand>
{
    public async Task<Result> Handle(DeleteRegistrationCommand request, CancellationToken cancellationToken)
    {
        var authorizationResult = await _authorizationService
            .AuthorizeAsync(_httpContextAccessor.HttpContext!.User, request.UserId, "SameUser");

        if (!authorizationResult.Succeeded)
            return Result.Unauthorized();

        var registration = await _registrationRepository
            .GetAsync(request.UserId, request.EventId, cancellationToken);

        if (registration is null)
        {
            return Result.NotFound(
                $"Registration for user {request.UserId} to event {request.EventId} not found"
            );
        }

        _registrationRepository.Delete(registration);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
