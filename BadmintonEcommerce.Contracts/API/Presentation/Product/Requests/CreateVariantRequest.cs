namespace BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;

public class CreateVariantRequest
{
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public List<OptionValueRequest> Values { get; set; }
}

public class OptionValueRequest
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
}