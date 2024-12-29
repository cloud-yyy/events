using Application.Abstractions;
using Ardalis.Result;
using Domain;
using Domain.Repositories;

namespace Application.Events.DeleteEvent;

public class DeleteEventCommandHandler(
    IEventRepository _eventRepository,
    IImageRepository _imageRepository,
    IUnitOfWork _unitOfWork
) : ICommandHandler<DeleteEventCommand>
{
    public async Task<Result> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);

        if (eventEntity is null)
            return Result.NotFound($"Event with id {request.Id} not found");

        if (eventEntity.Image is not null)
            _imageRepository.Delete(eventEntity.Image);

        _eventRepository.Delete(eventEntity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
