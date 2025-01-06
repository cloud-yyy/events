using Application.Abstractions;
using Application.Dtos;
using Application.ErrorResults;
using Ardalis.Result;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Events.UpdateEvent;

internal sealed class UpdateEventCommandHandler(
    IEventRepository _eventRepository,
    IUnitOfWork _unitOfWork,
    ICategoryRepository _categoryRepository,
    IMapper _mapper,
    IEmailSender _emailSender,
    LinkFactory _linkFactory
) : ICommandHandler<UpdateEventCommand, EventDto>
{
    public async Task<Result<EventDto>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdWithParticipantsAsync(request.Id, cancellationToken);
        if (eventEntity is null)
            return EventResults.NotFound.ById(request.Id);

        var existed = await _eventRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existed is not null && existed.Id != eventEntity.Id)
            return EventResults.Invalid.NameNotUnique(request.Name);

        var category = await _categoryRepository.GetByIdAsync(request.Category.Id, cancellationToken);
        if (category is null)
            return CategoryResults.NotFound.ById(request.Category.Id);

        eventEntity.Name = request.Name;
        eventEntity.Description = request.Description;
        eventEntity.Place = request.Place;
        eventEntity.Category = category;
        eventEntity.MaxParticipants = request.MaxParticipants;
        eventEntity.Date = request.Date;

        _eventRepository.Update(eventEntity);
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
