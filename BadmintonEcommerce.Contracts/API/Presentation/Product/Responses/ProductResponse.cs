namespace BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public string Brand { get; set; }
    public string Slug { get; set; }
    public string CategoryName { get; set; }
}