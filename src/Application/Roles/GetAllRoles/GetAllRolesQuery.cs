using Application.Abstractions;
using Application.Dtos;

namespace Application.Roles.GetAllRoles;

public record GetAllRolesQuery : IQuery<IEnumerable<RoleDto>>;
