using BadmintonEcommerce.Application.Features.Authentication.Login;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands;
using BadmintonEcommerce.Tests.Helpers.Constants;
using FluentAssertions;

namespace BadmintonEcommerce.Tests.Applications.Validators;

public class LoginCommandValidatorTest
{
    private readonly LoginCommandValidator validator = new();

    [Fact]
    public void Validate_EmptyEmail_ShouldReturnErrors()
    {
        LoginCommand loginCommandBuilder = new LoginCommandBuilder()
            .WithEmail("")
            .WithPassword("")
            .Build();
        
        var result = validator.Validate(loginCommandBuilder);
        result.IsValid.Should()
            .BeFalse();
        result.Errors.Should()
            .Contain(prop => prop.PropertyName == Authentication.Login.EmailField);
    }

    [Fact]
    public void Validate_EmptyPassword_ShouldReturnErrors()
    {
        LoginCommand loginCommandBuilder = new LoginCommandBuilder()
            .WithEmail("admin@gmail.com")
            .WithPassword("")
            .Build();
        
        var result = validator.Validate(loginCommandBuilder);
        result.IsValid.Should()
            .BeFalse();
        result.Errors.Should()
            .Contain(prop => prop.PropertyName == Authentication.Login.PasswordField);
    }
}