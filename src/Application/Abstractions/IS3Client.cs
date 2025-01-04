namespace Application.Abstractions;

public interface IS3Client
{
    public Task<string> UploadFileAsync(
        string bucketName,
        string objectKey,
        Stream fileStream,
        string contentType,
        CancellationToken cancellationToken = default
    );

    public Task<Stream> GetFileAsync(
        string bucketName,
        string objectKey,
        CancellationToken cancellationToken = default
    );

    public Task DeleteFileAsync(
        string bucketName,
        string objectKey,
        CancellationToken cancellationToken = default
    );

    public string GetPublicFileUrl(string bucketName, string objectKey);

    public Task EnsureBucketExistsAsync(
        string bucketName,
        CancellationToken cancellationToken = default
    );
}
