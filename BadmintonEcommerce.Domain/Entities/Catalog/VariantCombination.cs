namespace BadmintonEcommerce.Domain.Entities.Catalog;

public class VariantCombination
{
    //Variant
    public Guid VariantId { get; set; }
    public ProductVariant Variant { get; set; }
    
    //Option Value
    public Guid OptionValueId { get; set; }
    public ProductOptionValue OptionValue { get; set; }
}