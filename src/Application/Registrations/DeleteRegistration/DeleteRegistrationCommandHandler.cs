using System.Security.Claims;
using Application.Abstractions;
using Application.ErrorResults;
using Ardalis.Result;
using Domain;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace Application.Registrations.DeleteRegistration;

internal sealed class DeleteRegistrationCommandHandler(
    IRegistrationRepository _registrationRepository,
    IUnitOfWork _unitOfWork,
    IHttpContextAccessor _httpContextAccessor
) : ICommandHandler<DeleteRegistrationCommand>
{
    public async Task<Result> Handle(DeleteRegistrationCommand request, CancellationToken cancellationToken)
    {
        var userIdStr = _httpContextAccessor.HttpContext!.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userIdStr!);

        var registration = await _registrationRepository
            .GetAsync(userId, request.EventId, cancellationToken);

        if (registration is null)
            return RegistrationResults.NotFound.ByUserIdAndEventId(userId, request.EventId);

        _registrationRepository.Delete(registration);
        registration.Event!.CurrentParticipants -= 1;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
