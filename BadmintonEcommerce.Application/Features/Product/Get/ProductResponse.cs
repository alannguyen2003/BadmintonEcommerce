namespace BadmintonEcommerce.Application.Features.Product.Get;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public string Brand { get; set; }
    public Guid CategoryName { get; set; }
}