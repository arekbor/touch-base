using Arekbor.TouchBase.Domain.Entities;

namespace Arekbor.TouchBase.Application.Common.Interfaces;

public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
{
    Task<RefreshToken?> GetRefreshTokenByToken(string token, CancellationToken cancellationToken);
}