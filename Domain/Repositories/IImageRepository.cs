using Domain.Entities;

namespace Domain.Repositories;

public interface IImageRepository
{
    public Task<Image> GetByIdAsync(Guid id);
    public Task<Image> AddAsync(Image image);
    public Task DeleteAsync(Image image);
}
