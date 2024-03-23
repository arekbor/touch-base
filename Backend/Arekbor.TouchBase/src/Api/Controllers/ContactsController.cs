using Arekbor.TouchBase.Application.Contacts;
using Microsoft.AspNetCore.Mvc;

namespace Arekbor.TouchBase.Api.Controllers;

public class ContactsController : BaseApiController
{
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] ContactBody body, CancellationToken cancellationToken) 
        => await Send(new CreateOrUpdateContactCommand(body), cancellationToken);

    [HttpPut("update")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ContactBody body, CancellationToken cancellationToken)
        => await Send(new CreateOrUpdateContactCommand(body, id), cancellationToken);

    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] GetContactsQuery query, CancellationToken cancellationToken)
        => await Send(query, cancellationToken);

    [HttpGet("details")]
    public async Task<IActionResult> Details([FromQuery] GetContactQuery query, CancellationToken cancellationToken)
        => await Send(query, cancellationToken);

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery] DeleteContactCommand command, CancellationToken cancellationToken)
        => await Send(command, cancellationToken);
    
}