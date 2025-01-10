using Application.Dtos;
using Application.Registrations.CreateRegistration;
using Application.Registrations.DeleteRegistration;
using Application.Registrations.GetEventParticipantById;
using Application.Registrations.GetEventParticipants;
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

    [Authorize(Policy = PolicyNames.Admin)]
    [HttpGet]
    [Route("{userId:guid}")]
    public async Task<ActionResult<ParticipantDto>> GetByUserId(
        [FromRoute] Guid eventId,
        [FromRoute] Guid userId)
    {
        var query = new GetEventParticipantByIdQuery(eventId, userId);

        var result = await Sender.Send(query);
        
        return this.ToActionResult(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ParticipantDto>> Create
        ([FromRoute] Guid eventId)
    {
        var command = new CreateRegistrationCommand(eventId);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<ParticipantDto>> Delete
        ([FromRoute] Guid eventId)
    {
        var command = new DeleteRegistrationCommand(eventId);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }
}
