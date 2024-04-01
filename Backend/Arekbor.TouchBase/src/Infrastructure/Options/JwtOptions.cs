using System.ComponentModel.DataAnnotations;

namespace Arekbor.TouchBase.Infrastructure.Options;

public class JwtOptions
{
    public const string Position = "Jwt";
    [Required]
    public string Secret { get; set; } = "";
    [Required]
    public string Issuer { get; set; } = "";
    [Required]
    public string Audience { get; set; } = "";
    [Required]
    [Range(1, 3600)]
    public int ExpiresInSeconds { get; set; }
}