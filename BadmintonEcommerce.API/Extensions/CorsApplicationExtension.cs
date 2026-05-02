using SharedKernel.Constants;

namespace BadmintonEcommerce.API.Extensions;

public static class CorsApplicationExtension
{
    public static void UseCorsApplication(this IApplicationBuilder app)
    {
        app.UseCors(CorsConfiguration.AdminPolicy);
        app.UseCors(CorsConfiguration.ClientPolicy);
    }
}