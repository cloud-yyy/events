using Application.Abstractions;
using Application.Dtos;
using Application.ErrorResults;
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
            return EventResults.NotFound.ById(request.EventId);

        var registration = await _registrationRepository
            .GetParticipantByIdAsync(eventEntity.Id, request.UserId, cancellationToken);

        if (registration is null)
            return RegistrationResults.NotFound.ByUserIdAndEventId(request.UserId, request.EventId);

        return Result.Success(_mapper.Map<ParticipantDto>(registration));
    }
}
