using Arekbor.TouchBase.Application.Common.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace Arekbor.TouchBase.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IHostEnvironment environment)
    {
        services.AddControllers();

        services.AddProblemDetails(setup => {
            setup.IncludeExceptionDetails = (ctx, ex) => environment.IsDevelopment();

            setup.Map<NotFoundException>(ex => new ProblemDetails{
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            });

            setup.Map<BadRequestException>(ex => new ProblemDetails{
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest,
            });

            setup.Map<ModelsValidationException>(ex => new ProblemDetails{
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest,
                Extensions = ex.Errors!
            });

            setup.Map<UnauthorizedException>(ex => new ProblemDetails{
                Detail = ex.Message,
                Status = StatusCodes.Status401Unauthorized,
            });
        });
        
        services.AddSwaggerGen(config => {
            config.SwaggerDoc("v1", new OpenApiInfo{Title = "Touch Base API", Version = "v1"});
            config.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme{
                In = ParameterLocation.Header, 
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey 
            });

            var securityScheme = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            var securityRequirements = new OpenApiSecurityRequirement()
            {
                {securityScheme, new string[] { }},
            };

            config.AddSecurityRequirement(securityRequirements);
        });

        return services;
    }
}