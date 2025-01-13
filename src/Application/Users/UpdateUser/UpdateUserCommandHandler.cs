using Application.Abstractions;
using Application.Dtos;
using Application.ErrorResults;
using Ardalis.Result;
using AutoMapper;
using Domain.Authentication;
using Domain.Constants;
using Domain.Repositories;

namespace Application.Users.UpdateUser;

internal sealed class UpdateUserCommandHandler(
    IUserRepository _userRepository,
    IRoleRepository _roleRepository,
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    ICurrentUserAccessor _currentUserAccessor
) : ICommandHandler<UpdateUserCommand, UserDto>
{
    public async Task<Result<UserDto>> Handle
        (UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserAccessor.UserId is null)
            return Result.Unauthorized();

        var userId = (Guid)_currentUserAccessor.UserId;

        if (userId != request.Id && _currentUserAccessor.Role != RoleNames.Admin)
            return UserResults.Invalid.CannotUpdate(request.Id);

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return UserResults.NotFound.ById(userId);

        var role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
        if (role is null)
            return RoleResults.NotFound.ById(request.RoleId);

        if (role.Name == RoleNames.Admin && _currentUserAccessor.Role != RoleNames.Admin)
            return UserResults.Invalid.CannotGrantRole(RoleNames.Admin);

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Role = role;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(_mapper.Map<UserDto>(user));
    }
}
