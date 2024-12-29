namespace Domain.Entities;

public class Image
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; } = string.Empty;
}
