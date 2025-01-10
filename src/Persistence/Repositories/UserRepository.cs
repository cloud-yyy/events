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

    public User Add(User user)
    {
        _context.Users.Add(user);
        return user;
    }

    public User Update(User user)
    {
        _context.Users.Attach(user);
        _context.Users.Entry(user).State = EntityState.Modified;
        return user;
    }
}
