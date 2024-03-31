using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Arekbor.TouchBase.Infrastructure.Identity;
using Arekbor.TouchBase.Infrastructure.Persistence;
using Arekbor.TouchBase.Infrastructure.Persistence.Repositories;

namespace Arekbor.TouchBase.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    { 
        services.AddOptionsConfiguration();
        services.AddPersistenceConfiguration();
        services.AddIdentityConfiguration();

        //Repositories
        services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IContactRepository, ContactRepository>();
        services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();

        //Services
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}