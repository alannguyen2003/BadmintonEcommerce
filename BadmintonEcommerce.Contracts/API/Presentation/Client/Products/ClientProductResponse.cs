namespace BadmintonEcommerce.Contracts.API.Presentation.Client.Products;

public class ClientProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public Dictionary<string, List<ProductOptionValueResponse>> Options { get; set; }
    public Dictionary<string, List<string>> Variants { get; set; }
}

public class ProductOptionValueResponse
{
    public Guid Id { get; set; }
    public string Value { get; set; }
}