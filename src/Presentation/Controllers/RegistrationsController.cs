using Application.Dtos;
using Application.Registrations.CreateRegistration;
using Application.Registrations.DeleteRegistration;
using Application.Registrations.GetParticipants;
using Ardalis.Result.AspNetCore;
using Domain;
using Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("/api/events/{eventId:guid}/registrations")]
public class RegistrationsController(ISender sender) : ApiController(sender)
{
    [Authorize(Policy = PolicyNames.Admin)]
    [HttpGet]
    public async Task<ActionResult<IPagedList<ParticipantDto>>> GetAll(
        [FromRoute] Guid eventId,
        int pageNumber = 1, 
        int pageSize = 2)
    {
        var query = new GetEventParticipantsQuery(eventId, pageNumber, pageSize);

        var result = await Sender.Send(query);
        
        return this.ToActionResult(result);
    }

    [Authorize]
    [HttpPost]
    [Route("{userId:guid}")]
    public async Task<ActionResult<ParticipantDto>> Create
        ([FromRoute] Guid eventId, [FromRoute] Guid userId)
    {
        var command = new CreateRegistrationCommand(userId, eventId);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }

    [Authorize]
    [HttpDelete]
    [Route("{userId:guid}")]
    public async Task<ActionResult<ParticipantDto>> Delete
        ([FromRoute] Guid eventId, [FromRoute] Guid userId)
    {
        var command = new DeleteRegistrationCommand(userId, eventId);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }
}
