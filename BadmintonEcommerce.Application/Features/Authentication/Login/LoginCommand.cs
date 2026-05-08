using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Contracts.API.Presentation.Authentication.Response;

namespace BadmintonEcommerce.Application.Features.Authentication.Login;

public class LoginCommand : ICommand<string>
{
     public string Email { get; set; }
     public string Password { get; set; }
}