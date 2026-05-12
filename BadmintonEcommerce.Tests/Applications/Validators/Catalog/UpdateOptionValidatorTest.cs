using BadmintonEcommerce.Application.Features.Product.UpdateOption;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands.Catalog;
using FluentValidation.TestHelper;

namespace BadmintonEcommerce.Tests.Applications.Validators.Catalog;

public class UpdateOptionValidatorTest
{
     private readonly UpdateOptionCommandValidator
        _validator;

    public UpdateOptionValidatorTest()
    {
        _validator = new UpdateOptionCommandValidator();
    }

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveAnyErrors()
    {
        // Arrange
        var command = new UpdateOptionCommandBuilder().Valid();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyProductId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateOptionCommandBuilder().Valid();

        command.ProductId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.ProductId);
    }

    [Fact]
    public void Validate_NullAddedOptions_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateOptionCommandBuilder().Valid();

        command.AddedOptions = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.AddedOptions);
    }

    [Fact]
    public void Validate_NullAddedVariants_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateOptionCommandBuilder().Valid();

        command.AddedVariants = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.AddedVariants);
    }

    [Fact]
    public void Validate_NullDeletedOptions_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateOptionCommandBuilder().Valid();

        command.DeletedOptions = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.DeletedOptions);
    }

    [Fact]
    public void Validate_NullDeletedVariants_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateOptionCommandBuilder().Valid();

        command.DeletedVariants = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.DeletedVariants);
    }

    [Fact]
    public void Validate_DeletedOptionsContainsEmptyGuid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateOptionCommandBuilder().Valid();

        command.DeletedOptions =
        [
            Guid.Empty
        ];

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrors();
    }

    [Fact]
    public void Validate_DeletedVariantsContainsEmptyGuid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateOptionCommandBuilder().Valid();

        command.DeletedVariants =
        [
            Guid.Empty
        ];

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrors();
    }

    [Fact]
    public void Validate_NoOperations_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateOptionCommand
        {
            ProductId = Guid.NewGuid(),
            AddedOptions = [],
            AddedVariants = [],
            DeletedOptions = [],
            DeletedVariants = []
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x);
    }

    [Fact]
    public void Validate_DuplicateOptionCodes_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateOptionCommandBuilder().Valid();

        command.AddedOptions =
        [
            new CreateOptionRequest
            {
                Code = "COLOR",
                Name = "Color",
                Values = ["Red"]
            },
            new CreateOptionRequest
            {
                Code = "COLOR",
                Name = "Color 2",
                Values = ["Blue"]
            }
        ];

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.AddedOptions);
    }

    [Fact]
    public void Validate_DuplicateDeletedOptionIds_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateOptionCommandBuilder().Valid();

        Guid duplicatedId = Guid.NewGuid();

        command.DeletedOptions =
        [
            duplicatedId,
            duplicatedId
        ];

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.DeletedOptions);
    }

    [Fact]
    public void Validate_DuplicateDeletedVariantIds_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateOptionCommandBuilder().Valid();

        Guid duplicatedId = Guid.NewGuid();

        command.DeletedVariants =
        [
            duplicatedId,
            duplicatedId
        ];

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.DeletedVariants);
    }

    [Fact]
    public void Validate_InvalidAddedOption_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateOptionCommandBuilder().Valid();

        command.AddedOptions =
        [
            new CreateOptionRequest
            {
                Code = "",
                Name = "",
                Values = []
            }
        ];

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrors();
    }

    [Fact]
    public void Validate_InvalidAddedVariant_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateOptionCommandBuilder().Valid();

        command.AddedVariants =
        [
            new CreateVariantRequest
            {
                Price = -1,
                Stock = -1,
                Values = []
            }
        ];

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrors();
    }
}