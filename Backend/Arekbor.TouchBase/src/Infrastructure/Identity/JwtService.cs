using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Domain.Entities;
using Arekbor.TouchBase.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Arekbor.TouchBase.Infrastructure.Identity;

public class JwtService : IJwtService
{
    private readonly RefreshTokenOptions _refreshTokenOptions;
    private readonly JwtOptions _jwtOptions;
    public JwtService(
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        IOptions<JwtOptions> jwtOptions) 
    {
        _refreshTokenOptions = refreshTokenOptions.Value;
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        var secretOption = _jwtOptions.Secret 
            ?? throw new Exception("Secret option not found while creating an access token");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretOption));
        
        var secret = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = (ClaimsIdentity)principal.Identity!,
            Expires = DateTime.Now.AddSeconds(_jwtOptions.ExpiresInSeconds),
            Audience = _jwtOptions.Audience,
            Issuer = _jwtOptions.Issuer,
            SigningCredentials = secret
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(Guid userId)
    {
        var utc = DateTime.UtcNow;

        var token = Convert
            .ToBase64String(RandomNumberGenerator.GetBytes(64));
        
        return new RefreshToken{
            UserId = userId,
            Token = token,
            Expires = DateTime.UtcNow.AddSeconds(_refreshTokenOptions.ExpiresInSeconds),
        };
    }
}