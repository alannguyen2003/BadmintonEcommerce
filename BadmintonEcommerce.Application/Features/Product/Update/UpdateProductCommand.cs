using BadmintonEcommerce.Application.Abstraction.Messaging;

namespace BadmintonEcommerce.Application.Features.Product.Update;

public class UpdateProductCommand : ICommand
{
    public Guid Id { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public string Brand { get; set; }
    public Guid CategoryId { get; set; }
}