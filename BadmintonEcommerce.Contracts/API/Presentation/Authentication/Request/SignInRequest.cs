namespace BadmintonEcommerce.Contracts.API.Presentation.Authentication.Request;

public class SignInRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}