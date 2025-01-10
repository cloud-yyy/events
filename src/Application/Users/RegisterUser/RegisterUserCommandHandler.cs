using Application.Abstractions;
using Application.Dtos;
using Application.ErrorResults;
using Ardalis.Result;
using AutoMapper;
using Domain.Authentication;
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
            return UserResults.Invalid.EmailNotUnique(request.Email);

        var role = await _roleRepository.GetByNameAsync(RoleNames.User, cancellationToken);

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
