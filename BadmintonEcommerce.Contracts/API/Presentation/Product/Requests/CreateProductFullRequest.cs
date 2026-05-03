namespace BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;

public class CreateProductFullRequest
{
    public string Name { get; set; }
    public Guid CategoryId { get; set; }
    public bool Status { get; set; }
    public string Description { get; set; }
    public string Options { get; set; }
    public string SkuRows { get; set; }
}