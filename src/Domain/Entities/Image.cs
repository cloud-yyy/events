namespace Domain.Entities;

public class Image
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ObjectKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}
