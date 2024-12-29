using Domain.Entities;

namespace Domain.Repositories;

public interface IEventRepository
{
    public Task<IPagedList<Event>> GetAllAsync
        (int pageNumber, int pageSize, CancellationToken token = default);
    public Task<IPagedList<Event>> GetByFilterAsync
        (EventFilter filter, int pageNumber, int pageSize, CancellationToken token = default);
    public Task<Event?> GetByNameAsync
        (string name, CancellationToken token = default);
    public Task<Event?> GetByIdAsync
        (Guid id, CancellationToken token = default);
    public Task<IPagedList<User>> GetParticipantsAsync
        (Guid eventId, int pageNumber, int pageSize, CancellationToken token = default);
    public void Add(Event @event);
    public void Update(Event @event);
    public void Delete(Event @event);
}
