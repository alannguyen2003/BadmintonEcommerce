using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Domain.Entities.Catalog;

public class ProductCategory : Entity<Guid>
{
    public string CategoryName { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public ProductCategory? ParentCategory { get; set; }
    
    public ICollection<Product> Products { get; set; }
}