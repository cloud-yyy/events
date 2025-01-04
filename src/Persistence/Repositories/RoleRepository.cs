using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class RoleRepository(
    ApplicationDbContext _context
) : IRoleRepository
{
    public Role Add(Role role)
    {
        _context.Roles.Add(role);
        return role;
    }

    public void Delete(Role role)
    {
        _context.Roles.Remove(role);
    }

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken token = default)
    {
        return await _context.Roles.ToListAsync(token);
    }

    public async Task<Role?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _context.Roles.FindAsync([id], token);
    }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken token = default)
    {
        return await _context.Roles
            .SingleOrDefaultAsync(r => r.Name == name, token);
    }
}
