using Application.Abstractions;
using Application.ErrorResults;
using Ardalis.Result;
using Domain;
using Domain.Repositories;

namespace Application.Roles.DeleteRole;

internal sealed class DeleteRoleCommandHandler(
    IRoleRepository _roleRepository,
    IUnitOfWork _unitOfWork
) : ICommandHandler<DeleteRoleCommand>
{
    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (role is null)
            return RoleResults.NotFound.ById(request.Id);

        _roleRepository.Delete(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.NoContent();
    }
}
