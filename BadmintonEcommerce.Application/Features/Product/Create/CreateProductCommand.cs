using BadmintonEcommerce.Application.Abstraction.Messaging;

namespace BadmintonEcommerce.Application.Features.Product.Create;

public sealed class CreateProductCommand : ICommand<Guid>
{
    public string ProductName { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public Guid CategoryId { get; set; }
}