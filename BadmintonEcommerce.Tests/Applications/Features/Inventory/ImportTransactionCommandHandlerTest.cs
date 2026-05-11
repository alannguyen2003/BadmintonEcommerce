using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.Inventory.Import;
using BadmintonEcommerce.Domain.Entities.Inventory;
using BadmintonEcommerce.Domain.Enums;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands.Inventory;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Tests.Applications.Features.Inventory;

public class ImportTransactionCommandHandlerTest
{
    private readonly Mock<IInventoryItemRepository>
        _inventoryItemRepositoryMock;

    private readonly Mock<IInventoryTransactionRepository>
        _inventoryTransactionRepositoryMock;

    private readonly Mock<IMapper>
        _mapperMock;

    private readonly Mock<IDateTimeProvider>
        _dateTimeProviderMock;

    private readonly ImportTransactionCommandHandler
        _handler;

    public ImportTransactionCommandHandlerTest()
    {
        _inventoryItemRepositoryMock =
            new Mock<IInventoryItemRepository>();

        _inventoryTransactionRepositoryMock =
            new Mock<IInventoryTransactionRepository>();

        _mapperMock =
            new Mock<IMapper>();

        _dateTimeProviderMock =
            new Mock<IDateTimeProvider>();

        _handler =
            new ImportTransactionCommandHandler(
                _inventoryItemRepositoryMock.Object,
                _mapperMock.Object,
                _inventoryTransactionRepositoryMock.Object,
                _dateTimeProviderMock.Object);
    }

    [Fact]
    public async Task Handle_InventoryNotFound_ShouldReturnFailure()
    {
        // Arrange
        Guid inventoryId = Guid.NewGuid();
        
        var command = new ImportInventoryTransactionCommandBuilder()
            .WithInventoryItemId(inventoryId)
            .WithType(InventoryTransactionType.Import)
            .WithQuantity(10)
            .Build();

        _inventoryItemRepositoryMock
            .Setup(x => x.GetById(inventoryId))
            .Returns((InventoryItem?)null);

        // Act
        Result<Guid> result =
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeEquivalentTo(
            InventoryItemError.NotFound(inventoryId));

        _inventoryTransactionRepositoryMock.Verify(
            x => x.Insert(It.IsAny<InventoryTransaction>()),
            Times.Never);

        _inventoryItemRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ExportQuantityGreaterThanInventory_ShouldReturnFailure()
    {
        // Arrange
        Guid inventoryId = Guid.NewGuid();

        var inventory = new InventoryItem
        {
            Id = inventoryId,
            Quantity = 5
        };
        
        var command = new ImportInventoryTransactionCommandBuilder()
            .WithInventoryItemId(inventoryId)
            .WithType(InventoryTransactionType.Export)
            .WithQuantity(10)
            .Build();

        _inventoryItemRepositoryMock
            .Setup(x => x.GetById(inventoryId))
            .Returns(inventory);

        // Act
        Result<Guid> result =
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeEquivalentTo(
            InventoryItemError.OutOfQuantity(inventoryId));

        inventory.Quantity.Should().Be(5);

        _inventoryTransactionRepositoryMock.Verify(
            x => x.Insert(It.IsAny<InventoryTransaction>()),
            Times.Never);

        _inventoryItemRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ImportTransaction_ShouldIncreaseQuantity()
    {
        // Arrange
        Guid inventoryId = Guid.NewGuid();

        DateTime utcNow = DateTime.UtcNow;

        var inventory = new InventoryItem
        {
            Id = inventoryId,
            Quantity = 10
        };
        
        var command = new ImportInventoryTransactionCommandBuilder()
            .WithInventoryItemId(inventoryId)
            .WithType(InventoryTransactionType.Import)
            .WithQuantity(5)
            .Build();

        _inventoryItemRepositoryMock
            .Setup(x => x.GetById(inventoryId))
            .Returns(inventory);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(utcNow);

        InventoryTransaction? insertedTransaction = null;

        _inventoryTransactionRepositoryMock
            .Setup(x => x.Insert(
                It.IsAny<InventoryTransaction>()))
            .Callback<InventoryTransaction>(
                transaction => insertedTransaction = transaction);

        // Act
        Result<Guid> result =
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        inventory.Quantity.Should().Be(15);

        insertedTransaction.Should().NotBeNull();

        insertedTransaction!.Quantity.Should().Be(5);

        insertedTransaction.Type.Should()
            .Be(InventoryTransactionType.Import);

        insertedTransaction.InventoryItemId
            .Should().Be(inventoryId);

        insertedTransaction.CreatedOnUtc
            .Should().Be(utcNow);

        _inventoryTransactionRepositoryMock.Verify(
            x => x.Insert(It.IsAny<InventoryTransaction>()),
            Times.Once);

        _inventoryItemRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ExportTransaction_ShouldDecreaseQuantity()
    {
        // Arrange
        Guid inventoryId = Guid.NewGuid();

        DateTime utcNow = DateTime.UtcNow;

        var inventory = new InventoryItem
        {
            Id = inventoryId,
            Quantity = 20
        };
        
        var command = new ImportInventoryTransactionCommandBuilder()
            .WithInventoryItemId(inventoryId)
            .WithType(InventoryTransactionType.Export)
            .WithQuantity(5)
            .Build();

        _inventoryItemRepositoryMock
            .Setup(x => x.GetById(inventoryId))
            .Returns(inventory);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(utcNow);

        InventoryTransaction? insertedTransaction = null;

        _inventoryTransactionRepositoryMock
            .Setup(x => x.Insert(
                It.IsAny<InventoryTransaction>()))
            .Callback<InventoryTransaction>(
                transaction => insertedTransaction = transaction);

        // Act
        Result<Guid> result =
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        inventory.Quantity.Should().Be(15);

        insertedTransaction.Should().NotBeNull();

        insertedTransaction!.Type.Should()
            .Be(InventoryTransactionType.Export);

        insertedTransaction.Quantity.Should().Be(5);

        insertedTransaction.InventoryItemId
            .Should().Be(inventoryId);

        insertedTransaction.CreatedOnUtc
            .Should().Be(utcNow);

        _inventoryTransactionRepositoryMock.Verify(
            x => x.Insert(It.IsAny<InventoryTransaction>()),
            Times.Once);

        _inventoryItemRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }
}