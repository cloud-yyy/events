using Application.Dtos;
using Application.Events.CreateEvent;
using Application.Events.DeleteEvent;
using Application.Events.DeleteEventImage;
using Application.Events.GetAllEvents;
using Application.Events.GetEventById;
using Application.Events.GetEventByName;
using Application.Events.UpdateEvent;
using Application.Events.UploadEventImage;
using Ardalis.Result.AspNetCore;
using Domain;
using Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Requests;

namespace Presentation.Controllers;

[Route("/api/events")]
public class EventsController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    [Route("", Name = "GetAllEvents")]
    public async Task<ActionResult<IPagedList<EventDto>>> GetAll(
        int pageNumber = 1, 
        int pageSize = 2,
        DateOnly? date = null, 
        string? place = null, 
        string? category = null)
    {
        var query = new GetAllEventsQuery(pageNumber, pageSize, date, place, category);

        var result = await Sender.Send(query);

        return this.ToActionResult(result);
    }

    [HttpGet]
    [Route("{id:guid}", Name = "GetEventById")]
    public async Task<ActionResult<EventDto>> GetById([FromRoute] Guid id)
    {
        var query = new GetEventByIdQuery(id);

        var result = await Sender.Send(query);

        return this.ToActionResult(result);
    }

    [HttpGet]
    [Route("{name}")]
    public async Task<ActionResult<EventDto>> GetByName([FromRoute] string name)
    {
        var query = new GetEventByNameQuery(name);

        var result = await Sender.Send(query);

        return this.ToActionResult(result);
    }

    [Authorize(Policy = PolicyNames.Admin)]
    [HttpPost]
    public async Task<ActionResult<EventDto>> Create([FromBody] CreateEventRequest request)
    {
        var command = new CreateEventCommand(
            request.Name,
            request.Description,
            request.Place,
            request.Category,
            request.MaxParticipants,
            request.Date
        );

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }

    [Authorize(Policy = PolicyNames.Admin)]
    [HttpPut]
    [Route("{id:guid}")]
    public async Task<ActionResult<EventDto>> Update([FromRoute] Guid id, [FromBody] UpdateEventRequest request)
    {
        var command = new UpdateEventCommand(
            id,
            request.Name,
            request.Description,
            request.Place,
            request.Category,
            request.MaxParticipants,
            request.Date
        );

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }

    [HttpPost]
    [Route("{id:guid}/image")]
    public async Task<ActionResult<ImageDto>> UploadEventImageAsync([FromRoute] Guid id, IFormFile file)
    {
        var command = new UploadEventImageCommand(id, file);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }

    [HttpDelete]
    [Route("{id:guid}/image")]
    public async Task<ActionResult> DeleteEventImageAsync([FromRoute] Guid id)
    {
        var command = new DeleteEventImageCommand(id);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }

    [Authorize(Policy = PolicyNames.Admin)]
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteEventCommand(id);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }
}
