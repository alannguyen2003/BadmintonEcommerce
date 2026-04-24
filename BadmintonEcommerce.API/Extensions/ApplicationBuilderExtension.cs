namespace BadmintonEcommerce.API.Extensions;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseSwaggerWithUi(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}