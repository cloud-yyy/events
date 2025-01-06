using Application.Abstractions;
using Application.Dtos;
using Application.ErrorResults;
using Ardalis.Result;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Roles.CreateRole;

internal sealed class CreateRoleCommandHandler(
    IRoleRepository _roleRepository,
    IMapper _mapper,
    IUnitOfWork _unitOfWork
) : ICommandHandler<CreateRoleCommand, RoleDto>
{
    public async Task<Result<RoleDto>> Handle
        (CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var existed = await _roleRepository.GetByNameAsync(request.Name, cancellationToken);
        
        if (existed is not null)
            return RoleResults.Invalid.NameNotUnique(request.Name);

        var role = new Role { Name = request.Name };

        _roleRepository.Add(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Created(_mapper.Map<RoleDto>(role));
    }
}
