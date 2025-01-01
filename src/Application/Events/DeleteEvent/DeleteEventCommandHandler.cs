using Application.Abstractions;
using Ardalis.Result;
using Domain;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Events.DeleteEvent;

public class DeleteEventCommandHandler(
    IEventRepository _eventRepository,
    IImageRepository _imageRepository,
    IUnitOfWork _unitOfWork,
    IEmailSender _emailSender,
    LinkFactory _linkFactory
) : ICommandHandler<DeleteEventCommand>
{
    public async Task<Result> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);

        if (eventEntity is null)
            return Result.NotFound($"Event with id {request.Id} not found");

        if (eventEntity.Image is not null)
            _imageRepository.Delete(eventEntity.Image);

        _eventRepository.Delete(eventEntity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await SendEmailNotifications(eventEntity.Participants, eventEntity);

        return Result.NoContent();
    }

    private async Task SendEmailNotifications(IEnumerable<User> recepients, Event eventEntity)
    {
        var emails = recepients.Select(u => u.Email);
        await _emailSender.SendManyAsync(
            emails, 
            $"Event {eventEntity.Name} was deleted", 
            $"Check our other <a href = '{_linkFactory.GenerateGetAllEventsUri()}'>events</a>!"
        );
    }
}
