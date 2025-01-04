namespace Presentation.Requests;

public record UpdateEventRequest(
    string Name, 
    string Description,
    string Place, 
    string Category,
    int MaxParticipants,
    DateOnly Date
);
