using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Arekbor.TouchBase.Infrastructure.Persistence;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistenceConfiguration(this IServiceCollection services) 
    {
        var persistenceOptions = services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<PersistenceOptions>>().Value;

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<ApplicationDbContext>(options => {
            options.UseNpgsql(persistenceOptions.Postgres, o => o.UseNodaTime());
        });

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());
        
        return services;
    }
}