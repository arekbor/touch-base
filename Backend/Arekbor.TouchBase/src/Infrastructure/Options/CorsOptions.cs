using System.ComponentModel.DataAnnotations;

namespace Arekbor.TouchBase.Infrastructure.Options;

public class CorsOptions
{
    public const string Position = "Cors";
    [Url]
    public string AllowedOrigins { get; init; } = "";
    public string AllowedMethods { get; init; } = "";
    public string AllowedHeaders { get; init; } = "";
    public bool AllowCredentials { get; init; }
    [Range(0, 100000)]
    public int MaxAgeInSeconds { get; init; }
}