using Application.Abstractions;
using Application.Dtos;

namespace Application.Events.UpdateEvent;

public record class UpdateEventCommand(
    Guid Id,
    string Name, 
    string Description,
    string Place, 
    int MaxParticipants,
    DateOnly Date,
    CategoryDto Category
) : ICommand<EventDto>;
