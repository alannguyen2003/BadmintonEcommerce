using BadmintonEcommerce.Application.Features.Authentication.Register;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands.Authentication;
using FluentValidation.TestHelper;

namespace BadmintonEcommerce.Tests.Applications.Validators.Authentication;

public class RegisterCommandValidatorTest
{
    private readonly RegisterCommandValidator _validator;

    public RegisterCommandValidatorTest()
    {
        _validator = new RegisterCommandValidator();
    }
    #region Email

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("invalid-email")]
    [InlineData("abc")]
    public void Validate_InvalidEmail_ShouldHaveValidationError(
        string email)
    {
        // Arrange
        var command = new RegisterCommandBuilder().ValidCommand();

        command.Email = email;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_ValidEmail_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new RegisterCommandBuilder().ValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    #endregion

    #region Fullname

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_EmptyFullname_ShouldHaveValidationError(
        string fullname)
    {
        // Arrange
        var command = new RegisterCommandBuilder().ValidCommand();

        command.Fullname = fullname;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Fullname);
    }

    [Fact]
    public void Validate_FullnameExceeds200Characters_ShouldHaveValidationError()
    {
        // Arrange
        var command = new RegisterCommandBuilder().ValidCommand();

        command.Fullname = new string('A', 201);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Fullname);
    }

    [Fact]
    public void Validate_ValidFullname_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new RegisterCommandBuilder().ValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Fullname);
    }

    #endregion

    #region Password

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("123")]
    [InlineData("1234567")]
    public void Validate_InvalidPassword_ShouldHaveValidationError(
        string password)
    {
        // Arrange
        var command = new RegisterCommandBuilder().ValidCommand();

        command.Password = password;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_PasswordExceeds32Characters_ShouldHaveValidationError()
    {
        // Arrange
        var command = new RegisterCommandBuilder().ValidCommand();

        command.Password = new string('A', 33);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_ValidPassword_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new RegisterCommandBuilder().ValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    #endregion

    #region Username

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("abc")]
    public void Validate_InvalidUsername_ShouldHaveValidationError(
        string username)
    {
        // Arrange
        var command = new RegisterCommandBuilder().ValidCommand();

        command.Username = username;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Validate_UsernameExceeds32Characters_ShouldHaveValidationError()
    {
        // Arrange
        var command = new RegisterCommandBuilder().ValidCommand();

        command.Username = new string('A', 33);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Validate_ValidUsername_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new RegisterCommandBuilder().ValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Username);
    }

    #endregion
}