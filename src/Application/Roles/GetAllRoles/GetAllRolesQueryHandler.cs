using Application.Abstractions;
using Application.Dtos;
using Ardalis.Result;
using AutoMapper;
using Domain.Repositories;

namespace Application.Roles.GetAllRoles;

public class GetAllRolesQueryHandler(
    IRoleRepository _roleRepository,
    IMapper _mapper
) : IQueryHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
{
    public async Task<Result<IEnumerable<RoleDto>>> Handle
        (GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllAsync(cancellationToken);

        return Result.Success(roles.Select(_mapper.Map<RoleDto>));
    }
}
