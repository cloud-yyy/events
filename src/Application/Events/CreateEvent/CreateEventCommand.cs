using Application.Abstractions;
using Application.Dtos;

namespace Application.Events.CreateEvent;

public record CreateEventCommand(
    string Name,
    string Description,
    string Place,
    int MaxParticipants,
    CategoryDto Category,
    DateOnly Date
) : ICommand<EventDto>;
