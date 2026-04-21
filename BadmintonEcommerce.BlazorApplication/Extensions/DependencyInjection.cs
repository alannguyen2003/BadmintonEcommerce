using BadmintonEcommerce.BlazorApplication.Services;

namespace BadmintonEcommerce.BlazorApplication.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddBlazorApplication(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddServices();

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ProductCatalog>();
        services.AddScoped<CartStateService>();
        services.AddScoped<OrderService>();
        return services;
    }
}