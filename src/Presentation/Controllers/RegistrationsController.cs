using Application.Dtos;
using Application.Registrations.CreateRegistration;
using Application.Registrations.DeleteRegistration;
using Application.Registrations.GetParticipants;
using Ardalis.Result.AspNetCore;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/events/{eventId:guid}/registrations")]
public class RegistrationsController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    public async Task<ActionResult<IPagedList<RegistrationDto>>> GetAll(
        [FromRoute] Guid eventId,
        int pageNumber = 1, 
        int pageSize = 2)
    {
        var query = new GetEventParticipantsQuery(eventId, pageNumber, pageSize);

        var result = await Sender.Send(query);
        
        return this.ToActionResult(result);
    }

    [HttpPost]
    [Route("{userId:guid}")]
    public async Task<ActionResult<RegistrationDto>> Create
        ([FromRoute] Guid eventId, [FromRoute] Guid userId)
    {
        var command = new CreateRegistrationCommand(userId, eventId);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }

    [HttpDelete]
    [Route("{userId:guid}")]
    public async Task<ActionResult<RegistrationDto>> Delete
        ([FromRoute] Guid eventId, [FromRoute] Guid userId)
    {
        var command = new DeleteRegistrationCommand(userId, eventId);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }
}
