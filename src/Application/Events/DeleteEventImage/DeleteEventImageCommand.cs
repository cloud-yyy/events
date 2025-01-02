using Application.Abstractions;

namespace Application.Events.DeleteEventImage;

public record DeleteEventImageCommand(
    Guid EventId
) : ICommand;
