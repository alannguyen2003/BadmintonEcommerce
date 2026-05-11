using BadmintonEcommerce.Application.Features.Inventory.Import;
using BadmintonEcommerce.Domain.Enums;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Commands.Inventory;

public class ImportInventoryTransactionCommandBuilder
{
    private Guid inventoryItemId;
    private InventoryTransactionType type;
    private int quantity;

    public ImportInventoryTransactionCommandBuilder WithInventoryItemId(Guid inventoryItemId)
    {
        this.inventoryItemId = inventoryItemId;
        return this;
    }

    public ImportInventoryTransactionCommandBuilder WithType(InventoryTransactionType type)
    {
        this.type = type;
        return this;
    }

    public ImportInventoryTransactionCommandBuilder WithQuantity(int quantity)
    {
        this.quantity = quantity;
        return this;
    }

    public ImportTransactionCommand Build() => new ImportTransactionCommand()
    {
        InventoryItemId = inventoryItemId,
        Type = type,
        Quantity = quantity
    };

    public ImportTransactionCommand Valid() => new ImportTransactionCommand()
    {
        InventoryItemId = Guid.NewGuid(),
        Type = InventoryTransactionType.Import,
        Quantity = 10
    };
}