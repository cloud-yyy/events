using Application.Abstractions;
using Application.Dtos;

namespace Application.Events.UpdateEvent;

public record class UpdateEventCommand(
    Guid Id,
    string Name, 
    string Description,
    string Place, 
    string Category,
    int MaxParticipants,
    DateTime Date,
    string ImageUrl
) : ICommand<EventDto>;
