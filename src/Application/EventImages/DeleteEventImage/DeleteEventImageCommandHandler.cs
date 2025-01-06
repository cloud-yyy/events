using Application.Abstractions;
using Application.ErrorResults;
using Ardalis.Result;
using Domain;
using Domain.Repositories;

namespace Application.EventImages.DeleteEventImage;

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
            return EventResults.NotFound.ById(request.EventId);

        if (eventEntity.Image is null)
            return EventResults.Invalid.HasNoImage(request.EventId);

        await _s3Client.DeleteFileAsync
            (eventEntity.Image.BucketName, eventEntity.Image.ObjectKey, cancellationToken);

        _imageRepository.Delete(eventEntity.Image);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
