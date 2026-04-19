using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Domain.Entities.Catalog;

public class ProductOptionValue : Entity<Guid>
{
    public string Value { get; set; }
    
    public Guid OptionId { get; set; }
    public ProductOption Option { get; set; }
}