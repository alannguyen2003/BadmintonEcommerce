using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Domain.Entities.Catalog;

public class Product : Aggregate<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    
    //Category
    public Guid CategoryId { get; set; }
    public ProductCategory Category { get; set; }
    
    //Options 
    public ICollection<ProductOption> Options { get; set; }
    
    
}