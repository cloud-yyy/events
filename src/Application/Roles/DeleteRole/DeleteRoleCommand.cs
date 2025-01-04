using Application.Abstractions;

namespace Application.Roles.DeleteRole;

public record DeleteRoleCommand(Guid Id) : ICommand;
