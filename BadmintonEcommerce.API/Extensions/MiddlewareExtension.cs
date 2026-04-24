using BadmintonEcommerce.API.Middlewares;

namespace BadmintonEcommerce.API.Extensions;

public static class MiddlewareExtension
{
    public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();
        return app;
    }
}