using BadmintonEcommerce.Contracts.API.Presentation.Authentication.Request;
using BadmintonEcommerce.Contracts.API.Presentation.Authentication.Response;

namespace BadmintonEcommerce.BlazorApplication.Abstraction.Services;

public interface IAuthHttpService
{
    public Task<string> Register(RegisterRequest request);
    public Task<string> Login(SignInRequest request);
}