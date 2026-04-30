using BadmintonEcommerce.Domain.Enums;
using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Domain.Entities.Inventory;

public class InventoryTransaction : Entity<Guid>
{
    public InventoryTransactionType Type { get; set; }
    public int Quantity { get; set; }
    
    #region Foreign Key
    
    public Guid InventoryItemId { get; set; }
    public InventoryItem Inventory { get; set; }
    
    #endregion
}