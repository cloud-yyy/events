using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using AutoMapper;
using Domain;
using Domain.Repositories;

namespace Application.Events.GetAllEvents;

public class GetAllEventsQueryHandler(
    IEventRepository _eventRepository,
    IMapper _mapper
) : IQueryHandler<GetAllEventsQuery, IPagedList<EventDto>>
{
    public async Task<Result<IPagedList<EventDto>>> Handle
        (GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var filter = new EventFilter(request.Date, request.Place, request.Category);

        var pagedEntities = await _eventRepository.GetAllAsync(
            request.PageNumber, 
            request.PageSize,
            filter,
            cancellationToken);

        var pagedDtos = pagedEntities.ConvertTo(e => _mapper.Map<EventDto>(e));

        return Result.Success(pagedDtos);
    }
}
