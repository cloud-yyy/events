namespace Domain.Repositories;

public record EventFilter(DateOnly? Date, string? Place, string? Category);
