using Application.Dtos;
using Application.Events.CreateEvent;
using Application.Events.DeleteEvent;
using Application.Events.GetAllEvents;
using Application.Events.GetEventById;
using Application.Events.GetEventByName;
using Application.Events.UpdateEvent;
using Ardalis.Result.AspNetCore;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Requests;

namespace Presentation.Controllers;

[Route("/api/events")]
public class EventsController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    // TODO: check what type is better to return, IActionResult or smth like this
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
    [Route("{id:guid}")]
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

    [HttpPost]
    public async Task<ActionResult<EventDto>> Create([FromBody] CreateEventRequest request)
    {
        var command = new CreateEventCommand(
            request.Name,
            request.Description,
            request.Place,
            request.Category,
            request.MaxParticipants,
            request.Date,
            request.ImageUrl
        );

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }

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
            request.Date,
            request.ImageUrl
        );

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteEventCommand(id);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }
}
