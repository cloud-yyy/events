using Application.Dtos;
using Application.Roles.CreateRole;
using Application.Roles.DeleteRole;
using Application.Roles.GetAllRoles;
using Ardalis.Result.AspNetCore;
using Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Requests;

namespace Presentation.Controllers;

[Route("/api/roles")]
public class RolesController(ISender sender) : ApiController(sender)
{
    [Authorize(Policy = PolicyNames.Admin)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetAll()
    {
        var query = new GetAllRolesQuery();

        var result = await Sender.Send(query);

        return this.ToActionResult(result);
    }

    [Authorize(Policy = PolicyNames.Admin)]
    [HttpPost]
    public async Task<ActionResult<RoleDto>> Create([FromBody] CreateRoleRequest request)
    {
        var command = new CreateRoleCommand(request.Name);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }

    [Authorize(Policy = PolicyNames.Admin)]
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteRoleCommand(id);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }
}
