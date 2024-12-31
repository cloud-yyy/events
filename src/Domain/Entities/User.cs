namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; } = string.Empty;

    public Guid RoleId { get; set; }
    public Role? Role { get; set; }

    public ICollection<Event> EventsParticipatedIn { get; set; } = [];
}
