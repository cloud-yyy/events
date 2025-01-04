using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using AutoMapper;
using Domain.Repositories;

namespace Application.Registrations.GetEventParticipantById;

public class GetEventParticipantByIdQueryHandler(
    IEventRepository _eventRepository,
    IRegistrationRepository _registrationRepository,
    IMapper _mapper
) : IQueryHandler<GetEventParticipantByIdQuery, ParticipantDto>
{
    public async Task<Result<ParticipantDto>> Handle
        (GetEventParticipantByIdQuery request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (eventEntity is null)
            return Result.NotFound($"Event with id {request.EventId} not found");

        var registration = await _registrationRepository
            .GetParticipantByIdAsync(eventEntity.Id, request.UserId, cancellationToken);

        if (registration is null)
            return Result.NotFound($"Participant with id {request.UserId} not found");

        return Result.Success(_mapper.Map<ParticipantDto>(registration));
    }
}
