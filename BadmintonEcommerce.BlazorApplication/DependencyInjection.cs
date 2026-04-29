namespace BadmintonEcommerce.BlazorApplication;

public static class DependencyInjection
{
    public static IServiceCollection AddBlazorApplication(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddApiHttpClient(configuration);

    public static IServiceCollection AddApiHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var apiClient = configuration["Client:Api"] ?? string.Empty;
        
        return services;
    }
}