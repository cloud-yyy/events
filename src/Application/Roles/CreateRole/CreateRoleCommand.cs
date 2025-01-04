using Application.Abstractions;
using Application.Dtos;

namespace Application.Roles.CreateRole;

public record CreateRoleCommand(string Name) : ICommand<RoleDto>;
