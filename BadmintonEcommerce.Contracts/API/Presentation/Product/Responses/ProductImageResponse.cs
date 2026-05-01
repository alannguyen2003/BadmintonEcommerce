namespace BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;

public class ProductImageResponse
{
    public string ImageUrl { get; set; }
    public string Metadata { get; set; }
    public bool IsMainProfile { get; set; }
    public string ProductName { get; set; }
}