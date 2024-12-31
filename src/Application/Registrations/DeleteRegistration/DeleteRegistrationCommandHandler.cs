using Application.Abstractions;
using Ardalis.Result;
using Domain;
using Domain.Repositories;

namespace Application.Registrations.DeleteRegistration;

public class DeleteRegistrationCommandHandler(
    IRegistrationRepository _registrationRepository,
    IUnitOfWork _unitOfWork
) : ICommandHandler<DeleteRegistrationCommand>
{
    public async Task<Result> Handle(DeleteRegistrationCommand request, CancellationToken cancellationToken)
    {
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
