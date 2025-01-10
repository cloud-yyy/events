using Domain.Repositories;

namespace Persistence;

public class UnitOfWork(
    ApplicationDbContext _context
) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
