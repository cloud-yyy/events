using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken token = default);
    public Task<User?> GetByEmailAsync(string email, CancellationToken token = default);
    public User Add(User user);
}
