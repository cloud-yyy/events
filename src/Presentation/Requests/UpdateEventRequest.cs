namespace Presentation.Requests;

public record UpdateEventRequest(
    Guid Id,
    string Name, 
    string Description,
    string Place, 
    string Category,
    int MaxParticipants,
    DateOnly Date
);
