using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class UserRepository(
    ApplicationDbContext _context
) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _context.Users
            .Include(u => u.Role)
            .SingleOrDefaultAsync(u => u.Id == id, token);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken token = default)
    {
        return await _context.Users
            .Include(u => u.Role)
            .SingleOrDefaultAsync(u => u.Email == email, token);
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }
}
