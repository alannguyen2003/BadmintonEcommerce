using BadmintonEcommerce.Application.Features.Product.Delete;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands.Catalog;
using FluentValidation.TestHelper;

namespace BadmintonEcommerce.Tests.Applications.Validators.Catalog;

public class DeleteProductValidatorTest
{
    private readonly DeleteProductCommandValidator _validator;

    public DeleteProductValidatorTest()
    {
        _validator = new DeleteProductCommandValidator();
    }

    [Fact]
    public void Validate_ValidProductId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new DeleteProductCommandBuilder()
            .WithProductId(Guid.NewGuid())
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(
            x => x.ProductId);
    }

    [Fact]
    public void Validate_EmptyProductId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeleteProductCommandBuilder()
            .WithProductId(Guid.Empty)
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
                x => x.ProductId)
            .WithErrorMessage(
                "ProductId is required.");
    }
}