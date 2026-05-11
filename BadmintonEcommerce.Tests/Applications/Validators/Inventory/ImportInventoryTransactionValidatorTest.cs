using BadmintonEcommerce.Application.Features.Inventory.Import;
using BadmintonEcommerce.Domain.Enums;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands.Inventory;
using FluentValidation.TestHelper;

namespace BadmintonEcommerce.Tests.Applications.Validators.Inventory;

public class ImportInventoryTransactionValidatorTest
{
    private readonly ImportTransactionCommandValidator
        _validator;

    public ImportInventoryTransactionValidatorTest()
    {
        _validator =
            new ImportTransactionCommandValidator();
    }

    #region InventoryItemId

    [Fact]
    public void Validate_EmptyInventoryItemId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new ImportInventoryTransactionCommandBuilder().Valid();

        command.InventoryItemId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
                x => x.InventoryItemId)
            .WithErrorMessage(
                "Inventory item id cannot be empty.");
    }

    [Fact]
    public void Validate_ValidInventoryItemId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new ImportInventoryTransactionCommandBuilder().Valid();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(
            x => x.InventoryItemId);
    }

    #endregion

    #region Type

    [Fact]
    public void Validate_InvalidTransactionType_ShouldHaveValidationError()
    {
        // Arrange
        var command = new ImportInventoryTransactionCommandBuilder().Valid();

        command.Type =
            (InventoryTransactionType)999;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
                x => x.Type)
            .WithErrorMessage(
                "Invalid inventory transaction type.");
    }

    [Fact]
    public void Validate_ValidTransactionType_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new ImportInventoryTransactionCommandBuilder().Valid();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(
            x => x.Type);
    }

    #endregion

    #region Quantity

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Validate_QuantityLessThanOrEqualToZero_ShouldHaveValidationError(
        int quantity)
    {
        // Arrange
        var command = new ImportInventoryTransactionCommandBuilder().Valid();

        command.Quantity = quantity;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.Quantity);
    }

    [Fact]
    public void Validate_QuantityGreaterThan10000_ShouldHaveValidationError()
    {
        // Arrange
        var command = new ImportInventoryTransactionCommandBuilder().Valid();

        command.Quantity = 10001;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.Quantity);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(9999)]
    [InlineData(10000)]
    public void Validate_ValidQuantity_ShouldNotHaveValidationError(
        int quantity)
    {
        // Arrange
        var command = new ImportInventoryTransactionCommandBuilder().Valid();

        command.Quantity = quantity;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(
            x => x.Quantity);
    }

    #endregion
}