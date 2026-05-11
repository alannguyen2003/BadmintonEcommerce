using BadmintonEcommerce.Application.Features.Product.Create;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands.Catalog;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace BadmintonEcommerce.Tests.Applications.Validators.Catalog;

public class CreateProductValidatorTest
{
    private readonly CreateProductValidator
        _validator;

    public CreateProductValidatorTest()
    {
        _validator =
            new CreateProductValidator();
    }

    #region ProductName

    [Fact]
    public void Validate_EmptyProductName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommandBuilder().Valid();

        command.ProductName = string.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.ProductName);
    }

    [Fact]
    public void Validate_ProductNameExceeds100Characters_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommandBuilder().Valid();

        command.ProductName = new string('A', 101);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.ProductName);
    }

    [Fact]
    public void Validate_ValidProductName_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommandBuilder().Valid();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(
            x => x.ProductName);
    }

    #endregion

    #region Description

    [Fact]
    public void Validate_DescriptionExceeds1000Characters_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommandBuilder().Valid();

        command.Description = new string('A', 1001);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.Description);
    }

    [Fact]
    public void Validate_EmptyDescription_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommandBuilder().Valid();

        command.Description = string.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(
            x => x.Description);
    }

    [Fact]
    public void Validate_ValidDescription_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommandBuilder().Valid();

        command.Description = "Yonex professional shoes";

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(
            x => x.Description);
    }

    #endregion

    #region Brand

    [Fact]
    public void Validate_EmptyBrand_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommandBuilder().Valid();

        command.Brand = string.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.Brand);
    }

    [Fact]
    public void Validate_BrandExceeds100Characters_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommandBuilder().Valid();

        command.Brand = new string('A', 101);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.Brand);
    }

    [Fact]
    public void Validate_ValidBrand_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommandBuilder().Valid();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(
            x => x.Brand);
    }

    #endregion

    #region CategoryId

    [Fact]
    public void Validate_EmptyCategoryId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommandBuilder().Valid();

        command.CategoryId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
                x => x.CategoryId)
            .WithErrorMessage(
                "Category id cannot be empty.");
    }

    [Fact]
    public void Validate_ValidCategoryId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommandBuilder().Valid();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(
            x => x.CategoryId);
    }

    #endregion

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var command = new CreateProductCommandBuilder().Valid();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}