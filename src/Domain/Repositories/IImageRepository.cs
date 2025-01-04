using Domain.Entities;

namespace Domain.Repositories;

public interface IImageRepository
{
    public Task<Image?> GetByIdAsync(Guid id, CancellationToken token = default);
    public Image Add(Image image);
    public void Delete(Image image);
}
