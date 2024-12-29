using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Events.UpdateEvent;

public class UpdateEventCommandHandler(
    IEventRepository _eventRepository,
    IImageRepository _imageRepository,
    IUnitOfWork _unitOfWork,
    IMapper _mapper
) : ICommandHandler<UpdateEventCommand, EventDto>
{
    public async Task<Result<EventDto>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);
        if (eventEntity is null)
            return Result.NotFound($"Event with id {request.Id} not found");

        eventEntity.Name = request.Name;
        eventEntity.Description = request.Description;
        eventEntity.Place = request.Place;
        eventEntity.Category = request.Category;
        eventEntity.MaxParticipants = request.MaxParticipants;
        eventEntity.Date = request.Date;

        if (request.ImageUrl != eventEntity.Image?.Url)
        {
            if (eventEntity.Image is not null)
                _imageRepository.Delete(eventEntity.Image);

            var image = new Image { Url = request.ImageUrl };
            _imageRepository.Add(image);
            eventEntity.Image = image;
        }

        _eventRepository.Update(eventEntity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(_mapper.Map<EventDto>(eventEntity));
    }
}
