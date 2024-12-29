using Domain.Entities;

namespace Domain.Repositories;

public interface IRoleRepository
{
    public Task<IEnumerable<Role>> GetAllAsync(CancellationToken token = default);
    public Task<Role?> GetByIdAsync(Guid id, CancellationToken token = default);
    public Task<Role?> GetByNameAsync(string name, CancellationToken token = default);
    public void Add(Role role);
    public void Delete(Role role);
}
