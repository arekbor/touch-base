using Arekbor.TouchBase.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Arekbor.TouchBase.Api;

public static class ApplicationBuilder
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var services = app.ApplicationServices.CreateScope();
        var dbContext = services.ServiceProvider.GetService<ApplicationDbContext>() 
            ?? throw new Exception($"Error while applying migrations. Cannot find {nameof(ApplicationDbContext)} service.");

        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
    }
}
