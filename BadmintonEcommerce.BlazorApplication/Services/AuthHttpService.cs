using System.Text.Json;
using BadmintonEcommerce.BlazorApplication.Abstraction.Services;
using BadmintonEcommerce.Contracts.API.Presentation.Authentication.Request;
using BadmintonEcommerce.Contracts.API.Presentation.Authentication.Response;
using BadmintonEcommerce.Contracts.Endpoints;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using static BadmintonEcommerce.Contracts.Endpoints.AuthenticationEndpoint;

namespace BadmintonEcommerce.BlazorApplication.Services;

public class AuthHttpService : IAuthHttpService
{
    private readonly HttpClient client;

    public AuthHttpService(IHttpClientFactory httpClientFactory)
    {
        client = httpClientFactory.CreateClient("api");
    }
    public async Task<string> Register(RegisterRequest request)
    {
        var result = await client.PostAsJsonAsync<string>(
            AuthenticationEndpoint.Register, JsonSerializer.Serialize(request));
        return result.Content.ToString();
    }

    public async Task<string> Login(SignInRequest request)
    {
        var result = await client.PostAsJsonAsync(
            AuthenticationEndpoint.Login, request);
        var content = await result.Content.ReadFromJsonAsync<string>();
        return content;
    }
}