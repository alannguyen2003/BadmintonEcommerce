using BadmintonEcommerce.BlazorApplication.Extensions;

namespace BadmintonEcommerce.BlazorApplication;

public static class DependencyInjection
{
    public static IServiceCollection AddBlazorApplication(this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddHttpClient(configuration)
            .AddDependency();
}