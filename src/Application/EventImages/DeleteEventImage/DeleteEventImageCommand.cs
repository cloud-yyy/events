using Application.Abstractions;

namespace Application.EventImages.DeleteEventImage;

public record DeleteEventImageCommand(
    Guid EventId
) : ICommand;
