using BadmintonEcommerce.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace BadmintonEcommerce.API.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();
        
    }
}