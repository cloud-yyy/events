using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories;

public class ImageRepository(
    ApplicationDbContext _context
) : IImageRepository
{
    public async Task<Image?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _context.Images.FindAsync([id], token);
    }

    public void Add(Image image)
    {
        _context.Images.Add(image);
    }

    public void Delete(Image image)
    {
        _context.Images.Remove(image);
    }
}
