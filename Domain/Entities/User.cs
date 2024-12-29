namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string SecondName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; } = string.Empty;

    public Guid RoleId { get; set; }
    public Role? Role { get; set; }

    public IEnumerable<Event> EventsParticipatedIn { get; set; } = [];
}
