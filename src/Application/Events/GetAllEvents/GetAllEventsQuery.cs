using Application.Abstractions;
using Application.Dtos;
using Domain;

namespace Application.Events.GetAllEvents;

public record class GetAllEventsQuery
    (int PageNumber, int PageSize, DateOnly Date, string Place, string Category)
    : IQuery<IPagedList<EventDto>>;
