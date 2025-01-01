using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using AutoMapper;
using Domain;
using Domain.Repositories;

namespace Application.Registrations.GetParticipants;

public class GetEventParticipantsQueryHandler(
    IEventRepository _eventRepository,
    IRegistrationRepository _registrationRepository,
    IMapper _mapper
) : IQueryHandler<GetEventParticipantsQuery, IPagedList<ParticipantDto>>
{
    public async Task<Result<IPagedList<ParticipantDto>>> Handle
        (GetEventParticipantsQuery request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);

        if (@event is null)
            return Result.NotFound($"Event with id {request.EventId} not found");

        var registrations = await _registrationRepository.GetParticipantsAsync
            (@event.Id, request.PageNumber, request.PageSize, cancellationToken);

        return Result.Success(registrations.ConvertTo(e => _mapper.Map<ParticipantDto>(e)));
    }
}
