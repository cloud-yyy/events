using Application.Abstractions;

namespace Application.Registrations.DeleteRegistration;

public record DeleteRegistrationCommand(Guid UserId, Guid EventId) : ICommand;
