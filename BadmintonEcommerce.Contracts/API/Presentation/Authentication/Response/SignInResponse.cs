namespace BadmintonEcommerce.Contracts.API.Presentation.Authentication.Response;

public class SignInResponse
{
    public int Role { get; set; }
    public string Token { get; set; }
}