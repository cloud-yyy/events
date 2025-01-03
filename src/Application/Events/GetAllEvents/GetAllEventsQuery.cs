using Application.Abstractions;
using Application.Dtos;
using Domain;

namespace Application.Events.GetAllEvents;

public record class GetAllEventsQuery(
    int PageNumber, 
    int PageSize, 
    DateOnly? Date = null, 
    string? Place = null, 
    string? Category = null
) : IQuery<IPagedList<EventDto>>;
