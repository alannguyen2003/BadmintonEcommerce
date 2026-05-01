using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Domain.Entities.Catalog;

public class ProductImage : Entity<Guid>
{
    public string Url { get; set; }
    public string? ImageMetadata { get; set; }
    public bool IsPrimary { get; set; }
    
    //Product
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}