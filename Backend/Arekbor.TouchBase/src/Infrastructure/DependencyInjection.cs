using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Arekbor.TouchBase.Infrastructure.Identity;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Arekbor.TouchBase.Infrastructure.Persistence;
using Arekbor.TouchBase.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Arekbor.TouchBase.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    { 
        //Options
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Position));
        services.Configure<CorsOptions>(configuration.GetSection(CorsOptions.Position));
        services.Configure<PersistenceOptions>(configuration.GetSection(PersistenceOptions.Position));
        services.Configure<RefreshTokenOptions>(configuration.GetSection(RefreshTokenOptions.Position));

        //Databases
        var persistenceOptions = services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<PersistenceOptions>>().Value;

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<ApplicationDbContext>(options => {
            options.UseNpgsql(persistenceOptions.Postgres);
        });

        //Repositories
        services.TryAddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.TryAddTransient<IUserRepository, UserRepository>();
        services.TryAddTransient<IContactRepository, ContactRepository>();
        services.TryAddTransient<IRefreshTokenRepository, RefreshTokenRepository>();

        //Auth
        var jwtOptions = services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<JwtOptions>>().Value;

        var secretOption = jwtOptions.Secret
            ?? throw new Exception("Secret option not found while creating the Authentication");

        var keyBytes = Encoding.ASCII.GetBytes(secretOption);

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

        //Services
        services.TryAddScoped<IIdentityService, IdentityService>();
        services.TryAddScoped<IJwtService, JwtService>();

        return services;
    }
}