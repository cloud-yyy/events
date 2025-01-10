using Domain;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class CategoryRepository(
    ApplicationDbContext _context
) : ICategoryRepository
{
    public async Task<IPagedList<Category>> GetAllAsync
        (int pageNumber, int pageSize, CancellationToken token = default)
    {
        IQueryable<Category> query = _context.Categories
            .Include(c => c.Events);

        return await PagedList<Category>.CreateAsync(query, pageNumber, pageSize, token);
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _context.Categories
            .Include(c => c.Events)
            .SingleOrDefaultAsync(c => c.Id == id, token);
    }

    public async Task<Category?> GetByNameAsync(string name, CancellationToken token = default)
    {
        return await _context.Categories
            .Include(c => c.Events)
            .SingleOrDefaultAsync(c => c.Name == name, token);
    }

    public void Add(Category category)
    {
        _context.Categories.Add(category);
    }

    public void Update(Category category)
    {
        _context.Categories.Attach(category);
        _context.Categories.Entry(category).State = EntityState.Modified;
    }

    public void Delete(Category category)
    {
        _context.Categories.Remove(category);
    }
}
