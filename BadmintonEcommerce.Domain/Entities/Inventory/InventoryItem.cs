using BadmintonEcommerce.Domain.Entities.Catalog;
using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Domain.Entities.Inventory;

public class InventoryItem : Aggregate<Guid>
{
    //Variant
    public Guid VariantId { get; set; }
    public ProductVariant Variant { get; set; }
    
    public int Quantity { get; set; }
    public int Reserved { get; set; }
    
    public int Available => Quantity - Reserved;

    /*public void Import(int quantity)
    {
        Quantity += quantity;
    }

    public void Reserve(int quantity)
    {
        
    }*/
    
    
}