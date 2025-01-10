using Application.Abstractions;
using Application.ErrorResults;
using Ardalis.Result;
using Domain.Authentication;
using Domain.Repositories;

namespace Application.Registrations.DeleteRegistration;

internal sealed class DeleteRegistrationCommandHandler(
    IRegistrationRepository _registrationRepository,
    IUnitOfWork _unitOfWork,
    ICurrentUserAccessor _currentUserAccessor
) : ICommandHandler<DeleteRegistrationCommand>
{
    public async Task<Result> Handle(DeleteRegistrationCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserAccessor.UserId is null)
            return Result.Unauthorized();

        var userId = (Guid)_currentUserAccessor.UserId;

        var registration = await _registrationRepository
            .GetByUserIdAndEventIdAsync(userId, request.EventId, cancellationToken);

        if (registration is null)
            return RegistrationResults.NotFound.ByUserIdAndEventId(userId, request.EventId);

        _registrationRepository.Delete(registration);
        registration.Event!.CurrentParticipants -= 1;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
