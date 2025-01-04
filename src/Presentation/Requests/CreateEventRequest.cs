using Application.Dtos;

namespace Presentation.Requests;

public record class CreateEventRequest(
    string Name,
    string Description,
    string Place,
    int MaxParticipants,
    CategoryDto Category,
    DateOnly Date
);
