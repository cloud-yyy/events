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
    IMapper _mapper,
    IEmailSender _emailSender,
    LinkFactory _linkFactory
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

        var imageToDelete = eventEntity.Image;
        var image = new Image { Url = request.ImageUrl };
        _imageRepository.Add(image);
        eventEntity.Image = image;
        
        _eventRepository.Update(eventEntity);

        if (imageToDelete is not null)
            _imageRepository.Delete(imageToDelete);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await SendEmailNotifications(eventEntity.Participants, eventEntity);

        return Result.Success(_mapper.Map<EventDto>(eventEntity));
    }

    private async Task SendEmailNotifications(IEnumerable<User> recepients, Event eventEntity)
    {
        var emails = recepients.Select(u => u.Email);
        await _emailSender.SendManyAsync(
            emails, 
            $"Event {eventEntity.Name} was updated", 
            $"Check it out: <a href = '{_linkFactory.GenerateGetEventByIdUri(eventEntity.Id)}'>{eventEntity.Name}</a>"
        );
    }
}
