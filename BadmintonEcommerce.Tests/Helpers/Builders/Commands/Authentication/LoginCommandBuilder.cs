using BadmintonEcommerce.Application.Features.Authentication.Login;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Commands.Authentication;

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

    public LoginCommand Valid() => new LoginCommand()
    {
        Email = Constants.Authentication.Login.LoginValid.Email,
        Password = Constants.Authentication.Login.LoginValid.Password
    };
}