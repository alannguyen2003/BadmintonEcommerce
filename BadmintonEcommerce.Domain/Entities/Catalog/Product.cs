using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Domain.Entities.Catalog;

public class Product : Aggregate<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public string Slug { get; set; }
    public bool Status { get; set; }
    
    #region Foreign Keys
    public Guid CategoryId { get; set; }
    public ProductCategory Category { get; set; }
    #endregion
    
    #region Collections 
    public ICollection<ProductOption> Options { get; set; }
    public ICollection<ProductImage> Images { get; set; }
    public ICollection<ProductVariant> Variants { get; set; } 
    #endregion  
    
}