using Application.Abstractions;
using Application.Dtos;

namespace Application.Events.GetEventByName;

public record class GetEventByNameQuery(string Name) : IQuery<EventDto>;
