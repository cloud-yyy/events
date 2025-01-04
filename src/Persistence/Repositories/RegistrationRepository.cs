using Domain;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class RegistrationRepository(
    ApplicationDbContext _context
) : IRegistrationRepository
{
    public async Task<IPagedList<Registration>> GetParticipantsAsync
        (Guid eventId, int pageNumber, int pageSize, CancellationToken token = default)
    {
        var query = _context.Registrations
            .Where(e => e.EventId == eventId)
            .Include(e => e.User);

        return await PagedList<Registration>.CreateAsync(query, pageNumber, pageSize, token);
    }

    public async Task<Registration?> GetParticipantByIdAsync
        (Guid eventId, Guid userId, CancellationToken token = default)
    {
        return await _context.Registrations
            .Include(e => e.User)
            .SingleOrDefaultAsync(r => r.EventId == eventId && r.UserId == userId, token);
    }

    public async Task<Registration?> GetAsync
        (Guid UserId, Guid EventId, CancellationToken cancellationToken = default)
    {
        return await _context.Registrations
            .Where(r => r.UserId == UserId && r.EventId == EventId)
            .Include(r => r.Event)
            .Include(r => r.User)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Registration Add(Registration registration)
    {
        _context.Registrations.Add(registration);
        return registration;
    }

    public void Delete(Registration registration)
    {
        _context.Registrations.Remove(registration);
    }
}
