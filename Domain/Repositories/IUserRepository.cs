using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    public Task<IPagedList<User>> GetEventParticipantsAsync(Guid eventId, int pageNumber, int pageSize);
    public Task<User> GetById(Guid eventId);
    public Task<User> AddAsync(User user);
}
