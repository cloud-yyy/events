namespace Domain.Entities;

public class Registration
{
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public DateTime RegistrationDate { get; set; }
}
