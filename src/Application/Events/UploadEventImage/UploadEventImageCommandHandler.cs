using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using Domain;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Events.UploadEventImage;

internal sealed class UploadEventImageCommandHandler(
    IS3Client _s3Client,
    IImageRepository _imageRepository,
    IEventRepository _eventRepository,
    IUnitOfWork _unitOfWork
) : ICommandHandler<UploadEventImageCommand, ImageDto>
{
    public async Task<Result<ImageDto>> Handle
        (UploadEventImageCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (eventEntity is null)
            return Result.NotFound($"Event with id {request.EventId} not found");

        using var stream = request.File.OpenReadStream();
        var contentType = request.File.ContentType;

        var id = Guid.NewGuid();
        var extension = Path.GetExtension(request.File.FileName);
        var objectKey = $"events/{id}{extension}";

        var publicUrl = await _s3Client.UploadFileAsync(
            "event-images",
            objectKey,
            stream,
            contentType,
            cancellationToken
        );

        var imageEntity = new Image
        {
            ObjectKey = objectKey,
            BucketName = "event-images",
            ContentType = contentType
        };

        _imageRepository.Add(imageEntity);
        eventEntity.Image = imageEntity;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Created(new ImageDto(id, publicUrl));
    }
}
