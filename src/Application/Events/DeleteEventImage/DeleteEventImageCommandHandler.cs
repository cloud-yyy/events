using Application.Abstractions;
using Ardalis.Result;
using Domain;
using Domain.Repositories;

namespace Application.Events.DeleteEventImage;

internal sealed class DeleteEventImageCommandHandler(
    IEventRepository _eventRepository,
    IImageRepository _imageRepository,
    IS3Client _s3Client,
    IUnitOfWork _unitOfWork
) : ICommandHandler<DeleteEventImageCommand>
{
    public async Task<Result> Handle
        (DeleteEventImageCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (eventEntity is null)
            return Result.NotFound($"Event with id {request.EventId} not found");

        if (eventEntity.Image is null)
            return Result.Invalid(new ValidationError($"Event with id {request.EventId} has no image"));

        await _s3Client.DeleteFileAsync
            (eventEntity.Image.BucketName, eventEntity.Image.ObjectKey, cancellationToken);

        _imageRepository.Delete(eventEntity.Image);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
