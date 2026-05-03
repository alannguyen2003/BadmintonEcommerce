using System.ComponentModel.DataAnnotations.Schema;
using BadmintonEcommerce.Domain.Entities.Inventory;
using BadmintonEcommerce.Domain.Entities.Order;
using BadmintonEcommerce.Domain.ValueObjects;
using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Domain.Entities.Catalog;

public class ProductVariant : Entity<Guid>
{
    public string SKU { get; set; }
    public decimal Price { get; set; }
    
    public Guid ProductId { get; set; }
    public Product Product { get; set; }

    private List<OptionValueId> _valueIds;

    [NotMapped]
    public IReadOnlyCollection<OptionValueId> ValueIds => _valueIds;
    public InventoryItem InventoryItem { get; set; }
    public ICollection<VariantCombination> Combinations { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}