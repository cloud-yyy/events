using Domain.Entities;

namespace Domain.Repositories;

public interface IRolesRepository
{
    public Task<IEnumerable<Role>> GetAllAsync();
    public Task<Role> GetByIdAsync(Guid id);
    public Task<Role> GetByNameAsync(Guid id);
    public Task<Role> AddAsync(Role role);
    public Task DeleteAsync(Guid id);
}
