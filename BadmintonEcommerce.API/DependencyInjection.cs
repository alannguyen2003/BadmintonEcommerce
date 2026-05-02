using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;

namespace BadmintonEcommerce.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddCorsPolicy(configuration);
        return services;
    }
}