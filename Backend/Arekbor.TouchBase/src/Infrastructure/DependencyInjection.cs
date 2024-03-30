using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Arekbor.TouchBase.Infrastructure.Identity;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Arekbor.TouchBase.Infrastructure.Persistence;
using Arekbor.TouchBase.Infrastructure.Persistence.Repositories;
using Arekbor.TouchBase.Infrastructure.Persistence.Extensions;

namespace Arekbor.TouchBase.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    { 
        //Options
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
        

        //Databases
        var persistenceOptions = services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<PersistenceOptions>>().Value;

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<ApplicationDbContext>(options => {
            options.UseNpgsql(persistenceOptions.Postgres, o => o.UseNodaTime());
        });

        //Repositories
        services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IContactRepository, ContactRepository>();
        services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();

        //Auth
        var jwtOptions = services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<JwtOptions>>().Value;

        var keyBytes = Encoding.ASCII.GetBytes(jwtOptions.Secret);

        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(config => {
            config.SaveToken = true;
            config.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ClockSkew = TimeSpan.Zero,
            };
        });

        services.AddAuthorizationBuilder();

        //Pagination
        var paginationOptions = services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<PaginationOptions>>();

        PaginatedListExtension.Configure(paginationOptions);

        //Services
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}