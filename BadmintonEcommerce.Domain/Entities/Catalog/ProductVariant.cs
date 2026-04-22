using BadmintonEcommerce.Domain.Entities.Inventory;
using BadmintonEcommerce.Domain.ValueObjects;
using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Domain.Entities.Catalog;

public class ProductVariant : Entity<Guid>
{
    public string SKU { get; set; }
    public decimal Price { get; set; }

    private List<OptionValueId> _valueIds;
    
    public Guid InventoryItemId { get; set; }
    public InventoryItem Inventory { get; set; }

    public IReadOnlyCollection<OptionValueId> ValueIds => _valueIds;
    public ICollection<VariantCombination> Combinations { get; set; }
}