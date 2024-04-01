using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Application.Common.Exceptions;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Arekbor.TouchBase.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private IApplicationDbContext _applicationDbContext;
    private readonly IJwtService _jwtService;
    public IdentityService(
        IApplicationDbContext applicationDbContext,
        IJwtService jwtService) 
    {
        _applicationDbContext = applicationDbContext;
        _jwtService = jwtService;
    }

    public async Task<(string accessToken, string refreshToken)> Authorize(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _applicationDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
                ?? throw new UnauthorizedException();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Role, user.Role.ToString()),
        };

        var accessToken = _jwtService.GenerateAccessToken(claims);

        var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
        
        if (refreshToken.Token is null)
            throw new UnauthorizedException();

        await _applicationDbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return (accessToken, refreshToken.Token);
    }

    public async Task<(string accessToken, string refreshToken)> Refresh(string oldToken, CancellationToken cancellationToken)
    {
        var refreshToken = await _applicationDbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == oldToken 
                && rt.Expires.ToUniversalTime() >= DateTime.UtcNow, cancellationToken)
                    ?? throw new UnauthorizedException();
        
        var tokens = await Authorize(refreshToken.UserId, cancellationToken);

        _applicationDbContext.RefreshTokens.Remove(refreshToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return tokens;
    }

    public string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyPasswordHash(string hash, string password)
        => BCrypt.Net.BCrypt.Verify(password, hash);
}