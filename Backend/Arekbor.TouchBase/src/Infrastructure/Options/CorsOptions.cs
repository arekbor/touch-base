using System.ComponentModel.DataAnnotations;

namespace Arekbor.TouchBase.Infrastructure.Options;

public class CorsOptions
{
    public const string Position = "Cors";
    [Required]
    [Url]
    public string AllowedOrigins { get; init; } = "";
    [Required]
    public string AllowedMethods { get; init; } = "";
    [Required]
    public string AllowedHeaders { get; init; } = "";
    [Required]
    public bool AllowCredentials { get; init; }
    [Required]
    [Range(0, 100000)]
    public int MaxAgeInSeconds { get; init; }
}