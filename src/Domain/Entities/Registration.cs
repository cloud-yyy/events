namespace Domain.Entities;

public class Registration
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid EventId { get; set; }
    public Event? Event { get; set; }

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
}
