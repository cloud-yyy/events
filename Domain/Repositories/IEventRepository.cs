using Domain.Entities;

namespace Domain.Repositories;

public interface IEventRepository
{
    public Task<IPagedList<Event>> GetAllAsync(int pageNumber, int pageSize);
    public Task<Event> GetByNameAsync(string name);
    public Task<Event> GetByIdAsync(Guid id);
    public Task<Event> AddAsync(Event @event);
    public Task<Event> UpdateAsync(Event @event);
    public Task DeleteAsync(Event @event);
}
