namespace Infrastructure.Options;

public class AwsOptions
{
    public string Region { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string ServiceURL { get; set; } = string.Empty;
    public string PublicURL { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
}
