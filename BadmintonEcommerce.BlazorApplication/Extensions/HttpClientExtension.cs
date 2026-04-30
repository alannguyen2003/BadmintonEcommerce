namespace BadmintonEcommerce.BlazorApplication.Extensions;

public static class HttpClientExtension
{
    public static IServiceCollection AddHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var apiClient = configuration["Client:Api"] ?? string.Empty;
        services.AddHttpClient("api", client =>
        {
            client.BaseAddress = new Uri(apiClient);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            /*
            client.DefaultRequestHeaders.Add("Accept", "application/multipart-data");
        */
        }); 
        return services;
    }
}