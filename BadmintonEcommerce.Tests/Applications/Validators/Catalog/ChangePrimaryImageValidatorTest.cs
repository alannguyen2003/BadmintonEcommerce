using BadmintonEcommerce.Application.Features.Product.ChangePrimaryImage;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands.Catalog;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace BadmintonEcommerce.Tests.Applications.Validators.Catalog;

public class ChangePrimaryImageValidatorTest
{
    private readonly ChangePrimaryImageCommandValidator _validator;

    public ChangePrimaryImageValidatorTest()
    {
        _validator = new ChangePrimaryImageCommandValidator();
    }

    [Fact]
    public void Validate_EmptyProductId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new ChangePrimaryImageCommandBuilder().Valid();

        command.ProductId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
                x => x.ProductId)
            .WithErrorMessage(
                "Product id cannot be empty.");
    }

    [Fact]
    public void Validate_ValidProductId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new ChangePrimaryImageCommandBuilder().Valid();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(
            x => x.ProductId);
    }

    [Fact]
    public void Validate_EmptyImageId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new ChangePrimaryImageCommandBuilder().Valid();

        command.ImageId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
                x => x.ImageId)
            .WithErrorMessage(
                "Image id cannot be empty.");
    }

    [Fact]
    public void Validate_ValidImageId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new ChangePrimaryImageCommandBuilder().Valid();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(
            x => x.ImageId);
    }

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var command = new ChangePrimaryImageCommandBuilder().Valid();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}