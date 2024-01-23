namespace Arekbor.TouchBase.Infrastructure.Options;

public class JwtOptions
{
    public const string Position = "Jwt";
    public string? Secret { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int ExpiresInSeconds { get; set; }
}