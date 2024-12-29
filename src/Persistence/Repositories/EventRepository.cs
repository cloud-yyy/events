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
        (int pageNumber, int pageSize, CancellationToken token = default)
    {
        var query = _context.Events.AsQueryable();
        return await PagedList<Event>.CreateAsync(query, pageNumber, pageSize, token);
    }

    public async Task<IPagedList<Event>> GetByFilterAsync
        (EventFilter filter, int pageNumber, int pageSize, CancellationToken token = default)
    {
        var query = _context.Events.AsQueryable();

        if (filter.Date is not null)
        {
            query = query.Where(e => DateOnly.FromDateTime(e.Date) == filter.Date);
        }

        if (filter.Category is not null)
        {
            query = query.Where(e => 
                string.Compare(e.Category, filter.Category, StringComparison.InvariantCultureIgnoreCase) == 0
            );
        }
    
        if (filter.Place is not null)
        {
            query = query.Where(e => 
                string.Compare(e.Place, filter.Place, StringComparison.InvariantCultureIgnoreCase) == 0
            );
        }

        return await PagedList<Event>.CreateAsync(query, pageNumber, pageSize, token);
    }

    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _context.Events.FindAsync([id], token);
    }

    public async Task<Event?> GetByNameAsync(string name, CancellationToken token = default)
    {
        return await _context.Events
            .SingleOrDefaultAsync(e => e.Name == name, token);
    }

    public async Task<IPagedList<User>> GetParticipantsAsync
        (Guid eventId, int pageNumber, int pageSize, CancellationToken token = default)
    {
        var query = _context.Events
            .Where(e => e.Id == eventId)
            .Include(e => e.Participants)
            .SelectMany(e => e.Participants);

        return await PagedList<User>.CreateAsync(query, pageNumber, pageSize, token);
    }

    public void Add(Event @event)
    {
        _context.Events.Add(@event);
    }

    public void Delete(Event @event)
    {
        _context.Events.Remove(@event);
    }

    public void Update(Event @event)
    {
        _context.Events.Attach(@event);
        _context.Entry(@event).State = EntityState.Modified;
    }
}
