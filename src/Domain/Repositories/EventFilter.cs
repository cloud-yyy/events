namespace Domain.Repositories;

public record EventFilter(
    DateOnly? Date = null, 
    string? Place = null, 
    string? Category = null
);
