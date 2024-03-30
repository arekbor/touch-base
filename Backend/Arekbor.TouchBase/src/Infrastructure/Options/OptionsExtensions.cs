using Microsoft.Extensions.DependencyInjection;

namespace Arekbor.TouchBase.Infrastructure.Options;

public static class OptionsExtensions 
{
    public static IServiceCollection AddOptionsConfiguration(this IServiceCollection services)
    {
        services
            .AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.Position)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddOptions<CorsOptions>()
            .BindConfiguration(CorsOptions.Position)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services
            .AddOptions<PersistenceOptions>()
            .BindConfiguration(PersistenceOptions.Position)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddOptions<RefreshTokenOptions>()
            .BindConfiguration(RefreshTokenOptions.Position)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddOptions<PaginationOptions>()
            .BindConfiguration(PaginationOptions.Position)
            .ValidateDataAnnotations()
            .ValidateOnStart();
            
        return services;
    }
}