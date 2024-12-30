namespace Application.Dtos;

public record class EventDto(
    Guid Id,
    string Name,
    string Description,
    string Place,
    string Category,
    int MaxParticipants,
    DateOnly Date,
    ImageDto Image
);
