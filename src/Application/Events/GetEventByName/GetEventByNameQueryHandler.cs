using Application.Abstractions;
using Application.Dtos;
using Application.ErrorResults;
using Ardalis.Result;
using AutoMapper;
using Domain.Repositories;

namespace Application.Events.GetEventByName;

internal sealed class GetEventByNameQueryHandler(
    IEventRepository _eventResository,
    IMapper _mapper
) : IQueryHandler<GetEventByNameQuery, EventDto>
{
    public async Task<Result<EventDto>> Handle(GetEventByNameQuery request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventResository.GetByNameAsync(request.Name, cancellationToken);

        if (eventEntity is null)
            return EventResults.NotFound.ByName(request.Name);

        return Result.Success(_mapper.Map<EventDto>(eventEntity));
    }
}
