using Domain.Entities;

namespace Domain.Repositories;

public interface IRegistrationRepository
{
    public Registration Add(Registration registration);
    public void Remove(Registration registration);
}
