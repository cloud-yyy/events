using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using AutoMapper;
using Domain;
using Domain.Constants;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler(
    IUserRepository _userRepository,
    IPasswordHasher _passwordHasher,
    IRoleRepository _roleRepository,
    IUnitOfWork _unitOfWork,
    IMapper _mapper
) : ICommandHandler<RegisterUserCommand, UserDto>
{
    public async Task<Result<UserDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        
        if (user is not null)
        {
            return Result.Invalid(
                new ValidationError(nameof(request.Email), $"User with email {request.Email} already exists")
            );
        }

        var role = await _roleRepository.GetByNameAsync(Roles.User, cancellationToken);

        user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            Role = role
        };

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Created(_mapper.Map<UserDto>(user));
    }
}
