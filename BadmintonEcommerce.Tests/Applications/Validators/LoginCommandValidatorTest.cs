using BadmintonEcommerce.Application.Features.Authentication.Login;
using BadmintonEcommerce.Application.Features.Authentication.Register;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands.Authentication;
using BadmintonEcommerce.Tests.Helpers.Constants;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace BadmintonEcommerce.Tests.Applications.Validators;

public class LoginCommandValidatorTest
{
    private readonly LoginCommandValidator _validator;

    public LoginCommandValidatorTest()
    {
        _validator = new LoginCommandValidator();
    }

    /*[Fact]
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
    }*/

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("invalid@")]
    public void Validate_InvalidEmail_ShouldHaveValidationError(string email)
    {
        LoginCommand command = new LoginCommandBuilder().Valid();
        
        command.Email = email;

        var result = _validator.TestValidate(command);
        
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    /*[Fact]
    public void Validate_EmptyPassword_ShouldReturnErrors()
    {
        LoginCommand loginCommandBuilder = new LoginCommandBuilder()
            .WithEmail("admin@gmail.com")
            .WithPassword("")
            .Build();
        
        var result = _validator.Validate(loginCommandBuilder);
        result.IsValid.Should()
            .BeFalse();
        result.Errors.Should()
            .Contain(prop => prop.PropertyName == Authentication.Login.PasswordField);
    }*/

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("123")]
    [InlineData("1234567")]
    public void Validate_InvalidPassword_ShouldHaveValidationError(string password)
    {
        LoginCommand command = new LoginCommandBuilder().Valid();
        
        command.Password = password;
        
        var result = _validator.TestValidate(command);
        
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_ValidEmailAndPassword_ShouldReturnSuccess()
    {
        LoginCommand loginCommandBuilder = new LoginCommandBuilder().Valid();

        var result = _validator.Validate(loginCommandBuilder);
        
        result.IsValid.Should()
            .BeTrue();
    }
}