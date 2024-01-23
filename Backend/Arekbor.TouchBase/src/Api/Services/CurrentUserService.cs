using System.Security.Claims;
using Arekbor.TouchBase.Application.Common.Interfaces;

namespace Arekbor.TouchBase.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _context;
    public CurrentUserService(IHttpContextAccessor context)
    {
        _context = context;
    }
    public string? Id 
        => _context.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}