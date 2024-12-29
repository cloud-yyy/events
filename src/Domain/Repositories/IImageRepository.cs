using Domain.Entities;

namespace Domain.Repositories;

public interface IImageRepository
{
    public Task<Image?> GetByIdAsync(Guid id, CancellationToken token = default);
    public void Add(Image image);
    public void Delete(Image image);
}
