using BadmintonEcommerce.Application.Abstraction.Messaging;

namespace BadmintonEcommerce.Application.Features.Authentication.Register;

public class RegisterCommand : ICommand<string>
{
    public string Fullname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
}