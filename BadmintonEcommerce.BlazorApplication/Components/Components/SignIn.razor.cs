using BadmintonEcommerce.BlazorApplication.Abstraction.Services;
using BadmintonEcommerce.Contracts.API.Presentation.Authentication.Request;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

namespace BadmintonEcommerce.BlazorApplication.Components.Components;

public partial class SignIn(IAuthHttpService authHttpService,
    IStorageService storageService) : ComponentBase
{
    [SupplyParameterFromForm]
    private SignInRequest SignInRequest { get; set; } = new();
    private string? Token { get; set; }
    
    private async Task FormSubmitted()
    {
        Console.WriteLine(
            SignInRequest.Email + " " + SignInRequest.Password);
        var content = await authHttpService.Login(SignInRequest);
        /*
        await sessionStorage.SetAsync("token", content.Token);
        */
        Token = content;
        storageService.StoreCookie(content, "");
        navigationManager.NavigateTo("/");
    }
}