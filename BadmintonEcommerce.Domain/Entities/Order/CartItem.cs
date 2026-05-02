using BadmintonEcommerce.Domain.Entities.Catalog;
using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Domain.Entities.Order;

public class CartItem : Entity<Guid>
{
    public Guid VariantId { get; set; }
    public ProductVariant Variant { get; set; }
    
    public int Quantity { get; set; }
    public decimal ProvisionalPrice { get; set; }
}