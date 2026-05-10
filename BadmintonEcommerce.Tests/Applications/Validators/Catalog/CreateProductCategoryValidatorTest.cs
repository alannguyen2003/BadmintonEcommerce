using BadmintonEcommerce.Application.Features.ProductCategory.Create;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands.Catalog;
using FluentValidation.TestHelper;

namespace BadmintonEcommerce.Tests.Applications.Validators.Catalog;

public class CreateProductCategoryValidatorTest
{
    private readonly CreateProductCategoryCommandValidator _validator;

    public CreateProductCategoryValidatorTest()
    {
        _validator = new CreateProductCategoryCommandValidator();
    }

    #region CategoryName

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_EmptyCategoryName_ShouldHaveValidationError(
        string? categoryName)
    {
        // Arrange
        var command = new CreateProductCategoryCommandBuilder().Valid();
        
        command.CategoryName = categoryName;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryName);
    }

    [Fact]
    public void Validate_CategoryNameExceeds100Characters_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCategoryCommandBuilder().Valid();

        command.CategoryName = new string('A', 101);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryName);
    }

    [Fact]
    public void Validate_ValidCategoryName_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCategoryCommandBuilder().Valid();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.CategoryName);
    }

    #endregion

    #region ParentCategoryId

    [Fact]
    public void Validate_NullParentCategoryId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCategoryCommandBuilder().Valid();

        command.ParentCategoryId = null;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(
            x => x.ParentCategoryId);
    }

    [Fact]
    public void Validate_EmptyGuidParentCategoryId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCategoryCommandBuilder().Valid();

        command.ParentCategoryId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.ParentCategoryId)
            .WithErrorMessage(
                "ParentCategoryId cannot be empty GUID.");
    }

    [Fact]
    public void Validate_ValidParentCategoryId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCategoryCommandBuilder().Valid();

        command.ParentCategoryId = Guid.NewGuid();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(
            x => x.ParentCategoryId);
    }
    
    #endregion
}