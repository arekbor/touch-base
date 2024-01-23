namespace Arekbor.TouchBase.Application.Common.Interfaces;

public interface IIdentityService
{
    string HashPassword(string password);
    bool VerifyPasswordHash(string hash, string password);
    Task<(string accessToken, string refreshToken)> Authorize(Guid userId, CancellationToken cancellationToken);
    Task<(string accessToken, string refreshToken)> Refresh(string oldToken, CancellationToken cancellationToken);
}