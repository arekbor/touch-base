using Arekbor.TouchBase.Application.Contacts;
using Microsoft.AspNetCore.Mvc;

namespace Arekbor.TouchBase.Api.Controllers;

public class ContactsController : BaseApiController
{
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateContactCommand command, CancellationToken cancellationToken) 
        => await Send(command, cancellationToken);
}