namespace BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;

public class ChangePrimaryImageRequest
{
    public Guid ProductId { get; set; }
    public Guid ImageId { get; set; }
}