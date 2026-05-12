using BadmintonEcommerce.Application.Features.Product.CreateOption;
using FluentValidation.TestHelper;

namespace BadmintonEcommerce.Tests.Applications.Validators.Catalog;

public class CreateProductOptionValueValidatorTest
{
    private readonly CreateProductOptionValueCommandValidator _validator;

    public CreateProductOptionValueValidatorTest()
    {
        _validator = new CreateProductOptionValueCommandValidator();
    }

    private static CreateProductOptionValueCommand CreateValidCommand()
    {
        return new CreateProductOptionValueCommand()
        {
            ProductId = Guid.NewGuid(),
            OptionName = "Color",
            OptionValues = ["Red", "Blue"]
        };
    }

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyProductId_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.ProductId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }

    [Fact]
    public void Validate_NullOptionName_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.OptionName = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OptionName);
    }

    [Fact]
    public void Validate_EmptyOptionName_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.OptionName = string.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OptionName);
    }

    [Fact]
    public void Validate_OptionNameTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.OptionName = new string('A', 101);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OptionName);
    }

    [Fact]
    public void Validate_NullOptionValues_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.OptionValues = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OptionValues);
    }

    [Fact]
    public void Validate_EmptyOptionValues_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.OptionValues = [];

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OptionValues);
    }

    [Fact]
    public void Validate_OptionValueEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.OptionValues = [""];

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("OptionValues[0]");
    }

    [Fact]
    public void Validate_OptionValueTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.OptionValues = [new string('A', 101)];

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("OptionValues[0]");
    }

    [Fact]
    public void Validate_DuplicateOptionValues_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.OptionValues = ["Red", "Red"];

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OptionValues);
    }

    [Fact]
    public void Validate_DuplicateOptionValuesDifferentCase_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.OptionValues = ["Red", "red"];

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OptionValues);
    }

    [Fact]
    public void Validate_DuplicateOptionValuesWithSpaces_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.OptionValues = ["Red", " Red "];

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OptionValues);
    }
}