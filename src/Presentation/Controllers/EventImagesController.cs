using Application.Dtos;
using Application.EventImages.DeleteEventImage;
using Application.EventImages.UploadEventImage;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("/api/events")]
public class EventImagesController(ISender sender) : ApiController(sender)
{
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
}
