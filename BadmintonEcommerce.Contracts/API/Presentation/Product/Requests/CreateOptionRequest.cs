namespace BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;

public class CreateOptionRequest
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string[] Values { get; set; }
}