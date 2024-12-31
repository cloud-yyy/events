using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Registrations.CreateRegistration;

public class CreateRegistrationCommandHandler(
    IUserRepository _userRepository,
    IEventRepository _eventRepository,
    IRegistrationRepository _registrationRepository,
    IUnitOfWork _unitOfWork,
    IMapper _mapper
) : ICommandHandler<CreateRegistrationCommand, RegistrationDto>
{
    public async Task<Result<RegistrationDto>> Handle
        (CreateRegistrationCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.NotFound($"User with id {request.UserId} not found");

        var @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (@event is null)
            return Result.NotFound($"Event with id {request.EventId} not found");

        var registration = await _registrationRepository.GetAsync
            (request.UserId, request.EventId, cancellationToken);

        if (registration is not null)
        {
            return Result.Invalid(
                new ValidationError($"User {request.UserId} is already registered to event {request.EventId}")
            );
        }

        registration = new Registration
        {
            User = user,
            Event = @event
        };

        _registrationRepository.Add(registration);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Created(_mapper.Map<RegistrationDto>(registration));
    }
}
