using Arekbor.TouchBase.Api;
using Arekbor.TouchBase.Api.Services;
using Arekbor.TouchBase.Application;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Infrastructure;
using Arekbor.TouchBase.Infrastructure.Options;
using Arekbor.TouchBase.Infrastructure.Persistence;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, config) => {
    config.ReadFrom.Configuration(ctx.Configuration);
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddApiServices(builder.Environment);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();


var app = builder.Build();

app.UseProblemDetails();

app.UseSerilogRequestLogging();

app.UseCors(builder => {
    var corsOptions = app.Services
        .GetRequiredService<IOptions<CorsOptions>>().Value;

    if (corsOptions.AllowCredentials)
    {
        builder.AllowCredentials();
    } 
    else 
    {
        builder.DisallowCredentials();
    }

    builder.WithOrigins(corsOptions.AllowedOrigins ?? string.Empty)
        .WithMethods(corsOptions.AllowedMethods ?? string.Empty)
        .WithHeaders(corsOptions.AllowedHeaders ?? string.Empty)
        .SetPreflightMaxAge(TimeSpan.FromSeconds(corsOptions.MaxAgeInSeconds));
});

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.ApplyMigrations();

app.Run();