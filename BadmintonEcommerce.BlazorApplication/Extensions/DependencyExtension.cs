using BadmintonEcommerce.BlazorApplication.Abstraction.Services;
using BadmintonEcommerce.BlazorApplication.Services;

namespace BadmintonEcommerce.BlazorApplication.Extensions;

public static class DependencyExtension
{
    public static IServiceCollection AddDependency(this IServiceCollection services)
    {
        services.AddScoped<IProductCategoryHttpService, ProductCategoryHttpService>();
        services.AddScoped<IProductHttpService, ProductHttpService>();
        return services;
    }
}