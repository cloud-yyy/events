using Domain.Entities;

namespace Domain.Repositories;

public interface ICategoryRepository
{
    public Task<IPagedList<Category>> GetAllAsync
        (int pageNumber, int pageSize, CancellationToken token = default);
    public Task<Category?> GetByIdAsync
        (Guid id, CancellationToken token = default);
    public Task<Category?> GetByNameAsync
        (string name, CancellationToken token = default);
    public Category Add(Category category);
    public Category Update(Category category);
    public void Delete(Category category);
}
