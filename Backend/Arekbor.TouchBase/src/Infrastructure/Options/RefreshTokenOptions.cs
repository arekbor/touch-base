namespace Arekbor.TouchBase.Infrastructure.Options;

public class RefreshTokenOptions 
{
    public const string Position = "RefreshToken";
    public int ExpiresInSeconds { get; set; }
}