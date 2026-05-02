using SharedKernel.Constants;

namespace BadmintonEcommerce.API.Extensions;

public static class CorsExtension
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsConfiguration.AdminPolicy,
                policy =>
                {
                    policy.WithOrigins(configuration["Cors:Admin"] ?? string.Empty)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });
        
        services.AddCors(options =>
        {
            options.AddPolicy(CorsConfiguration.ClientPolicy,
                policy =>
                {
                    policy.WithOrigins(configuration["Cors:Client"] ?? string.Empty)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });
        return services;
    }
}