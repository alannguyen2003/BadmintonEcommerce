using BadmintonEcommerce.Application.Features.Authentication.Register;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Commands.Authentication;

public class RegisterCommandBuilder
{
    private string username;
    private string email;
    private string password;
    private string fullname;

    public RegisterCommandBuilder WithUsername(string username)
    {
        this.username = username;
        return this;
    }

    public RegisterCommandBuilder WithEmail(string email)
    {
        this.email = email;
        return this;
    }

    public RegisterCommandBuilder WithPassword(string password)
    {
        this.password = password;
        return this;
    }

    public RegisterCommandBuilder WithFullName(string fullname)
    {
        this.fullname = fullname;
        return this;
    }

    public RegisterCommand Build() => new RegisterCommand()
    {
        Username = this.username,
        Email = this.email,
        Password = this.password,
        Fullname = this.fullname
    };

    public RegisterCommand ValidCommand() => new RegisterCommand()
    {
        Username = "nguyenho",
        Fullname = "Ho Duong Trung Nguyen",
        Email = "nguyenho30112003@gmail.com",
        Password = "12345678"
    };
}