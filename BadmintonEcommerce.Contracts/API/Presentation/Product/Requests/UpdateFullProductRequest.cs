namespace BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;

public class UpdateFullProductRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    
}