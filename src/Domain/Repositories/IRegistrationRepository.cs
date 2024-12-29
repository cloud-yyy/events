using Domain.Entities;

namespace Domain.Repositories;

public interface IRegistrationRepository
{
    public void Add(Registration registration);
    public void Remove(Registration registration);
}
