using BadmintonEcommerce.Application.Features.Product.Update;
using FluentValidation.TestHelper;

namespace BadmintonEcommerce.Tests.Applications.Validators.Catalog;

public class UpdateProductValidatorTest
{
    private readonly UpdateProductCommandValidator _validator;

    public UpdateProductValidatorTest()
    {
        _validator = new UpdateProductCommandValidator();
    }

    private static UpdateProductCommand CreateValidCommand()
    {
        return new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            ProductName = "Yonex Astrox 99",
            ProductDescription = "High quality badminton racket",
            Brand = "Yonex",
            CategoryId = Guid.NewGuid()
        };
    }

    #region ProductName

    [Fact]
    public void Validate_EmptyProductName_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.ProductName = string.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductName);
    }

    [Fact]
    public void Validate_NullProductName_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.ProductName = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductName);
    }

    [Fact]
    public void Validate_ProductNameOver100Characters_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.ProductName = new string('A', 101);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductName);
    }

    [Fact]
    public void Validate_ValidProductName_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ProductName);
    }

    #endregion

    #region ProductDescription

    [Fact]
    public void Validate_ProductDescriptionOver1000Characters_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.ProductDescription = new string('A', 1001);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductDescription);
    }

    [Fact]
    public void Validate_ValidProductDescription_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ProductDescription);
    }

    [Fact]
    public void Validate_NullProductDescription_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.ProductDescription = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ProductDescription);
    }

    #endregion

    #region Brand

    [Fact]
    public void Validate_EmptyBrand_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Brand = string.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Brand);
    }

    [Fact]
    public void Validate_NullBrand_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Brand = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Brand);
    }

    [Fact]
    public void Validate_BrandOver100Characters_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Brand = new string('B', 101);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Brand);
    }

    [Fact]
    public void Validate_ValidBrand_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Brand);
    }

    #endregion

    #region Id

    [Fact]
    public void Validate_EmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Id = Guid.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validate_ValidId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    #endregion

    #region CategoryId

    [Fact]
    public void Validate_EmptyCategoryId_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.CategoryId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [Fact]
    public void Validate_ValidCategoryId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.CategoryId);
    }

    #endregion

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}