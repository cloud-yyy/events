using Application.Categories.CreateCategory;
using Application.Categories.DeleteCategory;
using Application.Categories.GetAllCategories;
using Application.Categories.GetCategoryById;
using Application.Categories.UpdateCategory;
using Application.Dtos;
using Ardalis.Result.AspNetCore;
using Domain;
using Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Requests;

namespace Presentation.Controllers;

[Route("/api/categories")]
public class CategoriesController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    public async Task<ActionResult<IPagedList<CategoryDto>>> GetAll(
        int pageNumber = 1, 
        int pageSize = 2)
    {
        var query = new GetAllCategoriesQuery(pageNumber, pageSize);

        var result = await Sender.Send(query);

        return this.ToActionResult(result);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult<CategoryDto>> GetById([FromRoute] Guid id)
    {
        var query = new GetCategoryByIdQuery(id);

        var result = await Sender.Send(query);

        return this.ToActionResult(result);
    }


    [Authorize(Policy = PolicyNames.Admin)]
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryRequest request)
    {
        var command = new CreateCategoryCommand(request.Name);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }

    [Authorize(Policy = PolicyNames.Admin)]
    [HttpPut]
    [Route("{id:guid}")]
    public async Task<ActionResult<CategoryDto>> Update(
        [FromRoute] Guid id, 
        [FromBody] UpdateCategoryRequest request)
    {
        var command = new UpdateCategoryCommand(id, request.Name);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }

    [Authorize(Policy = PolicyNames.Admin)]
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteCategoryCommand(id);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }
}
