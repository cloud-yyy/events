using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Domain;
using Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.S3;

public class AmazonAwsS3Client(
    IAmazonS3 _s3Client,
    ILogger<AmazonAwsS3Client> _logger,
    IOptions<AwsOptions> _options
) : IS3Client
{
    public async Task EnsureBucketExistsAsync(
        string bucketName, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
            if (bucketExists)
            {
                _logger.LogInformation($"Bucket {bucketName} already exists.");
            }
            else
            {
                var createBucketRequest = new PutBucketRequest
                {
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.PublicRead,
                };

                await _s3Client.PutBucketAsync(createBucketRequest, cancellationToken);
                _logger.LogInformation($"Successfully created bucket {bucketName}.");
            }

            var policy = $@"{{
            ""Version"": ""2012-10-17"",
            ""Statement"": [
                {{
                ""Sid"": ""AllowPublicRead"",
                ""Effect"": ""Allow"",
                ""Principal"": ""*"",
                ""Action"": [""s3:GetObject""],
                ""Resource"": [""arn:aws:s3:::{bucketName}/*""]
                }}
            ]
            }}";

            var putPolicyRequest = new PutBucketPolicyRequest
            {
                BucketName = bucketName,
                Policy = policy
            };

            await _s3Client.PutBucketPolicyAsync(putPolicyRequest, cancellationToken);
            _logger.LogInformation($"Public-read policy successfully attached to bucket {bucketName}.");
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError($"Error creating bucket {bucketName}: {ex.Message}");
            throw;
        }
    }

    public async Task<string> UploadFileAsync(
        string bucketName, 
        string objectKey, 
        Stream fileStream, 
        string contentType, 
        CancellationToken cancellationToken = default)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = objectKey,
            InputStream = fileStream,
            ContentType = contentType,
            CannedACL = S3CannedACL.PublicRead
        };

        var response = await _s3Client.PutObjectAsync(putRequest, cancellationToken);
        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            throw new AmazonS3Exception($"Failed to upload image to S3. Response: {response.ToString()}");

        return GetPublicFileUrl(bucketName, objectKey);
    }

    public async Task<Stream> GetFileAsync(
        string bucketName, 
        string objectKey, 
        CancellationToken cancellationToken = default)
    {
        var getRequest = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = objectKey
        };

        var response = await _s3Client.GetObjectAsync(getRequest, cancellationToken);
        return response.ResponseStream;
    }

    public async Task DeleteFileAsync(
        string bucketName, 
        string objectKey, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = objectKey
            };

            await _s3Client.DeleteObjectAsync(deleteObjectRequest, cancellationToken);
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError($"Error deleting file {objectKey} from bucket {bucketName}: {ex.Message}");
            throw;
        }
    }

    public string GetPublicFileUrl(string bucketName, string objectKey)
    {
        return $"{_options.Value.PublicURL}/{_options.Value.BucketName}/{objectKey}";
    }
}
