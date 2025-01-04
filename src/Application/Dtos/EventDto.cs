namespace Application.Dtos;

public record EventDto(
    Guid Id,
    string Name,
    string Description,
    string Place,
    int CurrentParticipants,
    int MaxParticipants,
    CategoryDto Category,
    DateOnly Date,
    ImageDto Image
);
