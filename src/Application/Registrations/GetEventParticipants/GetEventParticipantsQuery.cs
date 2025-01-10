using Application.Abstractions;
using Application.Dtos;
using Domain;

namespace Application.Registrations.GetEventParticipants;

public record GetEventParticipantsQuery(Guid EventId, int PageNumber, int PageSize) 
    : IQuery<IPagedList<ParticipantDto>>;
