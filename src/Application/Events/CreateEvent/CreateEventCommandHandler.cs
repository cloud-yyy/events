using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Events.CreateEvent;

internal sealed class CreateEventCommandHandler(
    IEventRepository _eventRepository,
    ICategoryRepository _categoryRepository,
    IUnitOfWork _unitOfWork,
    IMapper _mapper
) : ICommandHandler<CreateEventCommand, EventDto>
{
    public async Task<Result<EventDto>> Handle
        (CreateEventCommand request, CancellationToken cancellationToken)
    {
        if (await _eventRepository.GetByNameAsync(request.Name, cancellationToken) is not null)
        {
            return Result.Invalid(
                new ValidationError(nameof(request.Name), $"Event with name {request.Name} already exists")
            );
        }

        var category = await _categoryRepository.GetByIdAsync(request.Category.Id, cancellationToken);
        if (category is null)
        {
            return Result.Invalid(
                new ValidationError(nameof(request.Category.Name), $"Category with name {request.Category.Name} not found")
            );
        }

        var eventEntity = new Event
        {
            Name = request.Name,
            Description = request.Description,
            Place = request.Place,
            Category = category,
            MaxParticipants = request.MaxParticipants,
            Date = request.Date,
        };

        _eventRepository.Add(eventEntity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Created(_mapper.Map<EventDto>(eventEntity));
    }
}
