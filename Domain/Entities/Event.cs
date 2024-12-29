namespace Domain.Entities;

public class Event
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }

    public Guid? ImageId { get; set; }
    public Image? Image { get; set; }

    public IEnumerable<User> Participants { get; set; } = [];
}
