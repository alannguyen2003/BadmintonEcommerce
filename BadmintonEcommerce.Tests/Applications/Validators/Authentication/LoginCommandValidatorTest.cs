using BadmintonEcommerce.Application.Features.Authentication.Login;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands.Authentication;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace BadmintonEcommerce.Tests.Applications.Validators.Authentication;

public class LoginCommandValidatorTest
{
    private readonly LoginCommandValidator _validator;

    public LoginCommandValidatorTest()
    {
        _validator = new LoginCommandValidator();
    }

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