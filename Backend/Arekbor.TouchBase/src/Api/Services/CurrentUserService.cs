using System.Security.Claims;
using Ardalis.GuardClauses;
using Arekbor.TouchBase.Application.Common.Interfaces;

namespace Arekbor.TouchBase.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _context;
    public CurrentUserService(IHttpContextAccessor context)
    {
        _context = context;
    }

    public Guid GetId()
    {
        var httpContext = _context.HttpContext;

        Guard.Against.Null(httpContext);

        var claim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

        Guard.Against.Null(claim);

        var id = claim.Value;

        Guard.Against.Null(id);

        if (!Guid.TryParse(id, out Guid guidId))
            throw new Exception($"Error while parsing {id} to GUID");

        return guidId;
    }

}