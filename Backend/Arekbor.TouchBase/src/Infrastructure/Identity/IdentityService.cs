using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Application.Common.Exceptions;
using System.Security.Claims;

namespace Arekbor.TouchBase.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    public IdentityService(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IJwtService jwtService) 
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<(string accessToken, string refreshToken)> Authorize(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(userId, cancellationToken)
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
        {
            throw new UnauthorizedException();
        }

        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return (accessToken, refreshToken.Token);
    }

    public async Task<(string accessToken, string refreshToken)> Refresh(string oldToken, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository
            .GetRefreshTokenByToken(oldToken, cancellationToken) 
                ?? throw new UnauthorizedException();
        
        var tokens = await Authorize(refreshToken.UserId, cancellationToken);

        _refreshTokenRepository.Delete(refreshToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return tokens;
    }

    public string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyPasswordHash(string hash, string password)
        => BCrypt.Net.BCrypt.Verify(password, hash);
}