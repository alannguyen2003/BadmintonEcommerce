using BadmintonEcommerce.Domain.Enums;
using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Domain.Entities.Catalog;

public class ProductOption : Entity<Guid>
{
    public string OptionName { get; set; }
    public OptionValueDataType DataType { get; set; }
    public string Code { get; set; }
    
    //Product
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    
    //Option Values
    public ICollection<ProductOptionValue> OptionValues { get; set; }
    
}