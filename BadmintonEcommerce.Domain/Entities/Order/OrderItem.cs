using BadmintonEcommerce.Domain.Entities.Catalog;
using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Domain.Entities.Order;

public class OrderItem : Entity<Guid>
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    
    public Guid VariantId { get; set; }
    public ProductVariant Variant { get; set; }
    
    public int Quantity { get; set; }
    public decimal ProvisionalCost { get; set; }
}