using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Arekbor.TouchBase.Infrastructure.Identity;
using Arekbor.TouchBase.Infrastructure.Persistence;

namespace Arekbor.TouchBase.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    { 
        services.AddOptionsConfiguration();
        services.AddPersistenceConfiguration();
        services.AddIdentityConfiguration();

        //Services
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}