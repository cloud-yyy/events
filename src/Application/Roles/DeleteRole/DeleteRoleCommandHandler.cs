using Application.Abstractions;
using Ardalis.Result;
using Domain;
using Domain.Repositories;

namespace Application.Roles.DeleteRole;

public class DeleteRoleCommandHandler(
    IRoleRepository _roleRepository,
    IUnitOfWork _unitOfWork
) : ICommandHandler<DeleteRoleCommand>
{
    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (role is null)
            return Result.NotFound($"Role with id {request.Id} not found");

        _roleRepository.Delete(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
