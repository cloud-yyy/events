using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Application.Registrations.CreateRegistration;

public class CreateRegistrationCommandHandler(
    IUserRepository _userRepository,
    IEventRepository _eventRepository,
    IRegistrationRepository _registrationRepository,
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    IAuthorizationService _authorizationService,
    IHttpContextAccessor _httpContextAccessor
) : ICommandHandler<CreateRegistrationCommand, ParticipantDto>
{
    public async Task<Result<ParticipantDto>> Handle
        (CreateRegistrationCommand request, CancellationToken cancellationToken)
    {
        var authorizationResult = await _authorizationService
            .AuthorizeAsync(_httpContextAccessor.HttpContext!.User, request.UserId, "SameUser");

        if (!authorizationResult.Succeeded)
            return Result.Unauthorized();

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

        if (@event.Participants.Count >= @event.MaxParticipants)
        {
            return Result.Invalid(
                new ValidationError($"Event {request.EventId} is full")
            );
        }

        registration = new Registration
        {
            User = user,
            Event = @event
        };

        _registrationRepository.Add(registration);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Created(_mapper.Map<ParticipantDto>(registration));
    }
}
