using Arekbor.TouchBase.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arekbor.TouchBase.Api.Controllers;

public class UsersController : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserQuery query, CancellationToken cancellationToken) 
        => await Send(query, cancellationToken);

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken) 
        => await Send(command, cancellationToken);

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenQuery query, CancellationToken cancellationToken) 
        => await Send(query, cancellationToken);

    [HttpGet("get")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken) 
        => await Send(new GetUserQuery(), cancellationToken);

    [HttpPut("updateProfile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command, CancellationToken cancellationToken)
        => await Send(command, cancellationToken);
}