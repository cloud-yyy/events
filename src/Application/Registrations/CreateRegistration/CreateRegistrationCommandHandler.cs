using Application.Abstractions;
using Application.Dtos;
using Application.ErrorResults;
using Ardalis.Result;
using AutoMapper;
using Domain.Authentication;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Registrations.CreateRegistration;

internal sealed class CreateRegistrationCommandHandler(
    IUserRepository _userRepository,
    IEventRepository _eventRepository,
    IRegistrationRepository _registrationRepository,
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    ICurrentUserAccessor _currentUserAccessor
) : ICommandHandler<CreateRegistrationCommand, ParticipantDto>
{
    public async Task<Result<ParticipantDto>> Handle
        (CreateRegistrationCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserAccessor.UserId is null)
            return Result.Unauthorized();

        var userId = (Guid)_currentUserAccessor.UserId;

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return UserResults.NotFound.ById(userId);

        var eventEntity = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (eventEntity is null)
            return EventResults.NotFound.ById(request.EventId);

        var registration = await _registrationRepository.GetByUserIdAndEventIdAsync
            (userId, request.EventId, cancellationToken);

        if (registration is not null)
            return RegistrationResults.Invalid.AlreadyRegistered(userId, request.EventId);

        if (eventEntity.CurrentParticipants >= eventEntity.MaxParticipants)
            return EventResults.Invalid.HasNoPlaces(request.EventId);

        registration = new Registration
        {
            User = user,
            Event = eventEntity
        };

        _registrationRepository.Add(registration);
        eventEntity.CurrentParticipants += 1;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Created(_mapper.Map<ParticipantDto>(registration));
    }
}
