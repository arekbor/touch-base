namespace Arekbor.TouchBase.Infrastructure.Options;

public class CorsOptions
{
    public const string Position = "Cors";
    public string? AllowedOrigins { get; set; }
    public string? AllowedMethods { get; set; }
    public string? AllowedHeaders { get; set; }
    public bool AllowCredentials { get; set; }
    public int MaxAgeInSeconds { get; set; }
}