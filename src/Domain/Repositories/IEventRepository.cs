using Domain.Entities;

namespace Domain.Repositories;

public interface IEventRepository
{
    public Task<IPagedList<Event>> GetAllAsync
        (int pageNumber, int pageSize, EventFilter? filter = null, CancellationToken token = default);
    public Task<Event?> GetByNameAsync
        (string name, CancellationToken token = default);
    public Task<Event?> GetByIdAsync
        (Guid id, CancellationToken token = default);

    public Task<Event?> GetByIdWithParticipantsAsync
        (Guid id, CancellationToken token = default);
    public Event Add(Event @event);
    public Event Update(Event @event);
    public void Delete(Event @event);
}
