namespace BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;

public class ProductDetailResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public bool Status { get; set; }
    public string CategoryName { get; set; }
    public Guid CategoryId { get; set; }
    public List<ProductOptionResponse> Options { get; set; }
    public List<ProductVariantResponse> Variants { get; set; }
    public List<ProductDetailImageResponse> Images { get; set; }
}

public class ProductOptionResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<ProductOptionValueResponse> Values { get; set; }
}

public class ProductOptionValueResponse
{
    public Guid Id { get; set; }
    public string Value { get; set; }
}

public class ProductVariantResponse
{
    public Guid Id { get; set; }
    public string SKU { get; set; }
    public decimal Price { get; set; }
    public List<Guid> OptionValues { get; set; }
}

public class ProductDetailImageResponse
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; }
}
