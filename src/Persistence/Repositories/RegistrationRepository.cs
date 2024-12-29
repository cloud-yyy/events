using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories;

public class RegistrationRepository(
    ApplicationDbContext _context
) : IRegistrationRepository
{
    public Registration Add(Registration registration)
    {
        _context.Set<Registration>().Add(registration);
        return registration;
    }

    public void Remove(Registration registration)
    {
        _context.Set<Registration>().Remove(registration);
    }
}
