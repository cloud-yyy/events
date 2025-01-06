using Application.Abstractions;
using Application.Dtos;
using Application.ErrorResults;
using Application.Options;
using Ardalis.Result;
using Domain;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Options;

namespace Application.EventImages.UploadEventImage;

internal sealed class UploadEventImageCommandHandler(
    IS3Client _s3Client,
    IImageRepository _imageRepository,
    IEventRepository _eventRepository,
    IUnitOfWork _unitOfWork,
    IOptions<AwsOptions> _options
) : ICommandHandler<UploadEventImageCommand, ImageDto>
{
    public async Task<Result<ImageDto>> Handle
        (UploadEventImageCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (eventEntity is null)
            return EventResults.NotFound.ById(request.EventId);

        using var stream = request.File.OpenReadStream();
        var contentType = request.File.ContentType;

        var id = Guid.NewGuid();
        var extension = Path.GetExtension(request.File.FileName);
        var objectKey = $"events/{id}{extension}";

        var publicUrl = await _s3Client.UploadFileAsync(
            _options.Value.BucketName,
            objectKey,
            stream,
            contentType,
            cancellationToken
        );

        var imageEntity = new Image
        {
            ObjectKey = objectKey,
            BucketName = _options.Value.BucketName,
            ContentType = contentType
        };

        _imageRepository.Add(imageEntity);
        eventEntity.Image = imageEntity;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Created(new ImageDto(id, publicUrl));
    }
}
