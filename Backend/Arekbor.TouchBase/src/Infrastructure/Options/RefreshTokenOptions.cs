using System.ComponentModel.DataAnnotations;

namespace Arekbor.TouchBase.Infrastructure.Options;

public class RefreshTokenOptions 
{
    public const string Position = "RefreshToken";
    [Required]
    [Range(1, 604_800)]
    public int ExpiresInSeconds { get; set; }
}