using BadmintonEcommerce.Contracts.API.Presentation.Authentication.Request;
using Microsoft.AspNetCore.Components;

namespace BadmintonEcommerce.BlazorApplication.Components.Pages;

public partial class AuthPage : ComponentBase
{
    private bool IsLogin { get; set; } = true;

    private SignInRequest SignInRequest { get; set; } = new();

    private void ChangeForm()
    {
        Console.WriteLine(IsLogin);
        IsLogin = !IsLogin;
    }
}