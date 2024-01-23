using System.Security.Claims;
using Arekbor.TouchBase.Domain.Entities;

namespace Arekbor.TouchBase.Application.Common.Interfaces;

public interface IJwtService
{
    public string GenerateAccessToken(IEnumerable<Claim> claims);
    public RefreshToken GenerateRefreshToken(Guid userId);
}