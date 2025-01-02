using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using AutoMapper;
using Domain.Repositories;

namespace Application.Events.GetEventById;

internal sealed class GetEventByIdQueryHandler(
    IEventRepository _eventResository,
    IMapper _mapper
) : IQueryHandler<GetEventByIdQuery, EventDto>
{
    public async Task<Result<EventDto>> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventResository.GetByIdAsync(request.Id, cancellationToken);

        if (eventEntity is null)
            return Result.NotFound($"Event with id {request.Id} not found");

        return Result.Success(_mapper.Map<EventDto>(eventEntity));
    }
}
