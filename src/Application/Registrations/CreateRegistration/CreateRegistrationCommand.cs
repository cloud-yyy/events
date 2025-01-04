using Application.Abstractions;
using Application.Dtos;

namespace Application.Registrations.CreateRegistration;

public record CreateRegistrationCommand(Guid EventId) : ICommand<ParticipantDto>;
