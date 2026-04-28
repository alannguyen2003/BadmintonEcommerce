namespace BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;

public class CreateProductRequest
{
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public string Brand { get; set; }
    public Guid CategoryId { get; set; }
}