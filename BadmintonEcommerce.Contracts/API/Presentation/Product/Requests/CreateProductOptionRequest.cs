namespace BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;

public class CreateProductOptionRequest
{
    public Guid ProductId { get; set; }
    public string OptionName { get; set; }
    public string[] OptionValues { get; set; }
}