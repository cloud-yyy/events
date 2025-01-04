using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Roles.CreateRole;

public class CreateRoleCommandHandler(
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
            return Result.Invalid(
                new ValidationError(nameof(request.Name), $"Role with name {request.Name} already exists")
            );

        var role = new Role { Name = request.Name };

        _roleRepository.Add(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(_mapper.Map<RoleDto>(role));
    }
}
