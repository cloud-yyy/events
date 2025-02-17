using Domain.Entities;

namespace Domain.Repositories;

public interface IRegistrationRepository
{
    public Task<IPagedList<Registration>> GetParticipantsAsync
        (Guid eventId, int pageNumber, int pageSize, CancellationToken token = default);
    public Task<Registration?> GetParticipantByIdAsync
        (Guid eventId, Guid userId, CancellationToken token = default);
    public Task<Registration?> GetByUserIdAndEventIdAsync
        (Guid UserId, Guid EventId, CancellationToken cancellationToken = default);
    public void Add(Registration registration);
    public void Delete(Registration registration);
}
