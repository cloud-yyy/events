using Application.Abstractions;
using Application.Dtos;

namespace Application.Events.GetEventById;

public record class GetEventByIdQuery(Guid Id) : IQuery<EventDto>;
