using Application.Abstractions;
using Application.Dtos;

namespace Application.Registrations.GetEventParticipantById;

public record GetEventParticipantByIdQuery(Guid EventId, Guid UserId)
     : IQuery<ParticipantDto>;
