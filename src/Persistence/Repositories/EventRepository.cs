using Domain;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class EventRepository(
    ApplicationDbContext _context
) : IEventRepository
{
    public async Task<IPagedList<Event>> GetAllAsync
        (int pageNumber, int pageSize, EventFilter? filter = null, CancellationToken token = default)
    {
        IQueryable<Event> query = _context.Events
            .Include(e => e.Image)
            .Include(e => e.Category);

        if (filter?.Date is not null)
            query = query.Where(e => e.Date == filter.Date);

        if (filter?.Category is not null)
            query = query.Where(e => e.Category!.Name.ToLower().Contains(filter.Category.ToLower()));
    
        if (filter?.Place is not null)
            query = query.Where(e => e.Place.ToLower().Contains(filter.Place.ToLower()));

        return await PagedList<Event>.CreateAsync(query, pageNumber, pageSize, token);
    }

    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _context.Events
            .Include(e => e.Image)
            .Include(e => e.Category)
            .SingleOrDefaultAsync(e => e.Id == id, token);
    }

    public async Task<Event?> GetByIdWithParticipantsAsync(Guid id, CancellationToken token = default)
    {
        return await _context.Events
            .Include(e => e.Image)
            .Include(e => e.Category)
            .Include(e => e.Participants)
            .SingleOrDefaultAsync(e => e.Id == id, token);
    }

    public async Task<Event?> GetByNameAsync(string name, CancellationToken token = default)
    {
        return await _context.Events
            .Include(e => e.Image)
            .Include(e => e.Category)
            .SingleOrDefaultAsync(e => e.Name == name, token);
    }

    public void Add(Event @event)
    {
        _context.Events.Add(@event);
    }

    public void Update(Event @event)
    {
        _context.Events.Attach(@event);
        _context.Events.Entry(@event).State = EntityState.Modified;
    }

    public void Delete(Event @event)
    {
        _context.Events.Remove(@event);
    }
}
