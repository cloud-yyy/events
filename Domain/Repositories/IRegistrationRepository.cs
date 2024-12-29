using Domain.Entities;

namespace Domain.Repositories;

public interface IRegistrationRepository
{
    public Task<Registration> RegisterAsync(Guid eventId, Guid userId);
    public Task<Registration> UnregisterAsync(Guid eventId, Guid userId);
}
