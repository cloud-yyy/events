using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Events.CreateEvent;

public class CreateEventCommandHandler(
    IEventRepository _eventRepository,
    IImageRepository _imageRepository,
    IUnitOfWork _unitOfWork,
    IMapper _mapper
) : ICommandHandler<CreateEventCommand, EventDto>
{
    public async Task<Result<EventDto>> Handle
        (CreateEventCommand request, CancellationToken cancellationToken)
    {
        if (_eventRepository.GetByNameAsync(request.Name, cancellationToken) is not null)
            return Result.Error($"Event with name {request.Name} already exists");

        // TODO: Add real image processing
        var image = new Image { Url = request.ImageUrl };
        _imageRepository.Add(image);

        var @event = new Event
        {
            Name = request.Name,
            Description = request.Description,
            Place = request.Place,
            Category = request.Category,
            MaxParticipants = request.MaxParticipants,
            Date = request.Date,
            Image = image
        };

        _eventRepository.Add(@event);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(_mapper.Map<EventDto>(@event));
    }
}
