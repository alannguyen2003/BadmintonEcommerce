using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BadmintonEcommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration)
        => services;
}