using Domain.Entities;

namespace Domain.Repositories;

public interface IRegistrationRepository
{
    public Task<IPagedList<Registration>> GetParticipantsAsync
        (Guid eventId, int pageNumber, int pageSize, CancellationToken token = default);
    public Task<Registration?> GetParticipantByIdAsync
        (Guid eventId, Guid userId, CancellationToken token = default);
    public Task<Registration?> GetAsync
        (Guid UserId, Guid EventId, CancellationToken cancellationToken = default);
    public Registration Add(Registration registration);
    public void Delete(Registration registration);
}
