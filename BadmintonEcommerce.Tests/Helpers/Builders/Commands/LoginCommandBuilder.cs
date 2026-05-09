using BadmintonEcommerce.Application.Features.Authentication.Login;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Commands;

public class LoginCommandBuilder
{
    private string email;
    private string password;

    public LoginCommandBuilder WithEmail(string email)
    {
        this.email = email;
        return this;
    }

    public LoginCommandBuilder WithPassword(string password)
    {
        this.password = password;
        return this;
    }

    public LoginCommand Build() => new LoginCommand()
    {
        Email = email,
        Password = password
    };
}