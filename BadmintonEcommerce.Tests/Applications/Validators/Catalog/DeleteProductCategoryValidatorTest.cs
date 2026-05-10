using BadmintonEcommerce.Application.Features.ProductCategory.Delete;
using FluentValidation.TestHelper;

namespace BadmintonEcommerce.Tests.Applications.Validators.Catalog;

public class DeleteProductCategoryValidatorTest
{
    private readonly DeleteProductCategoryValidator _validator;

    public DeleteProductCategoryValidatorTest()
    {
        _validator = new DeleteProductCategoryValidator();
    }

    [Fact]
    public void Validate_EmptyProductCategoryId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeleteProductCategoryCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
                x => x.ProductCategoryId)
            .WithErrorMessage(
                "Product Category Id cannot be empty.");
    }

    [Fact]
    public void Validate_ValidProductCategoryId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new DeleteProductCategoryCommand(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(
            x => x.ProductCategoryId);
    }
}