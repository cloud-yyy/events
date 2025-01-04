using Application.Dtos;

namespace Presentation.Requests;

public record UpdateEventRequest(
    string Name, 
    string Description,
    string Place, 
    int MaxParticipants,
    CategoryDto Category,
    DateOnly Date
);
