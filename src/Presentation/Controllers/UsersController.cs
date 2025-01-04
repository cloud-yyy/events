using Application.Dtos;
using Application.Users.LoginUser;
using Application.Users.RefreshToken;
using Application.Users.RegisterUser;
using Application.Users.UpdateUser;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Requests;

namespace Presentation.Controllers;

[Route("/api/users")]
public class UsersController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserRequest request)
    {
        var command = new RegisterUserCommand(request.FirstName, request.LastName, request.Email, request.Password);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginUserRequest request)
    {
        var command = new LoginUserCommand(request.Email, request.Password);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<ActionResult<TokenDto>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var command = new RefreshTokenCommand(request.AccessToken, request.RefreshToken);

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }

    [Authorize]
    [HttpPut]
    [Route("{id:guid}")]
    public async Task<ActionResult<UserDto>> RefreshToken
        ([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
    {
        var command = new UpdateUserCommand(
            id,
            request.FirstName,
            request.LastName,
            request.Role.Id
        );

        var result = await Sender.Send(command);

        return this.ToActionResult(result);
    }
}
