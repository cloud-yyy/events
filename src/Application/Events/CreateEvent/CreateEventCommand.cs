using Application.Abstractions;
using Application.Dtos;

namespace Application.Events.CreateEvent;

public record CreateEventCommand(
    string Name,
    string Description,
    string Place,
    string Category,
    int MaxParticipants,
    DateTime Date,
    // TODO: Add real images
    string ImageUrl
) : ICommand<EventDto>;
