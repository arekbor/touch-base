using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arekbor.TouchBase.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private IMediator Mediator => HttpContext.RequestServices.GetService<IMediator>() 
        ?? throw new Exception($"Error while getting {nameof(IMediator)} service");

    protected async Task<IActionResult> Send<TResult>(IRequest<TResult> request, CancellationToken cancellationToken)
        => Ok(await Mediator.Send(request, cancellationToken));
}