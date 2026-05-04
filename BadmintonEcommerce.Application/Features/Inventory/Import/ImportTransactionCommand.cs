using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Domain.Enums;

namespace BadmintonEcommerce.Application.Features.Inventory.Import;

public sealed class ImportTransactionCommand : ICommand<Guid>
{
    public Guid InventoryItemId { get; set; }
    public InventoryTransactionType Type { get; set; }
    public int Quantity { get; set; }
}