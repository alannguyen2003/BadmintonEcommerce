using BadmintonEcommerce.Infrastructure.Abstractions;
using BadmintonEcommerce.Infrastructure.DomainEvents;
using BadmintonEcommerce.Infrastructure.Persistence.Database;
using BadmintonEcommerce.Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Services;

namespace BadmintonEcommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration)
        => services;

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString(Constant.Connection.Database.DefaultConnection);

        services.AddDbContext<ApplicationDbContext>(options => options
            .UseSqlServer(connectionString)
            .UseSnakeCaseNamingConvention());
        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString(Constant.Connection.Database.DefaultConnection));
        return services;
    }
}