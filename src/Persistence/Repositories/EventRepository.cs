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
        var query = _context.Events
            .Include(e => e.Image)
            .AsQueryable();

        if (filter?.Date is not null)
            query = query.Where(e => e.Date == filter.Date);

        if (filter?.Category is not null)
            query = query.Where(e => e.Category.ToLower().Contains(filter.Category.ToLower()));
    
        if (filter?.Place is not null)
            query = query.Where(e => e.Place.ToLower().Contains(filter.Place.ToLower()));

        return await PagedList<Event>.CreateAsync(query, pageNumber, pageSize, token);
    }

    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _context.Events
            .Include(e => e.Image)
            .SingleOrDefaultAsync(e => e.Id == id, token);
    }

    public async Task<Event?> GetByNameAsync(string name, CancellationToken token = default)
    {
        return await _context.Events
            .Include(e => e.Image)
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

    public Event Add(Event @event)
    {
        _context.Events.Add(@event);
        return @event;
    }

    public void Delete(Event @event)
    {
        _context.Events.Remove(@event);
    }

    public Event Update(Event @event)
    {
        _context.Events.Attach(@event);
        _context.Entry(@event).State = EntityState.Modified;
        return @event;
    }
}
