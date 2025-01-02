namespace Application.Dtos;

public record class ImageDto
{
    public ImageDto()
    {
    }
    
    public ImageDto(Guid id, string publicUrl)
    {
        Id = id;
        PublicUrl = publicUrl;
    }

    public Guid Id { get; init; }
    public string PublicUrl { get; set; } = string.Empty;
};
