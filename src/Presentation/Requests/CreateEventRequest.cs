namespace Presentation.Requests;

public record class CreateEventRequest(
    string Name,
    string Description,
    string Place,
    string Category,
    int MaxParticipants,
    DateOnly Date
);
