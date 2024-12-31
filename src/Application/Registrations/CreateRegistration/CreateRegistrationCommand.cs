using Application.Abstractions;
using Application.Dtos;

namespace Application.Registrations.CreateRegistration;

public record CreateRegistrationCommand(Guid UserId, Guid EventId) : ICommand<RegistrationDto>;
