using BadmintonEcommerce.Application.Abstraction.Authentication;
using BadmintonEcommerce.Domain.Entities.Authentication;
using BadmintonEcommerce.Infrastructure.Persistence.Data;
using BadmintonEcommerce.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Services;

namespace BadmintonEcommerce.API.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();
        IPasswordHasher passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        IDateTimeProvider dateTimeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
        //Add Roles
        (List<Account>, List<Role>) authenticationContext = new AuthenticationData(
            passwordHasher, dateTimeProvider).Data();
        
        context.Accounts.AddRange(authenticationContext.Item1);
        context.Roles.AddRange(authenticationContext.Item2);

        context.SaveChanges();
    }
}