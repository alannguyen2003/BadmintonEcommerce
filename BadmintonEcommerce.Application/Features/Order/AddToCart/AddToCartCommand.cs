using BadmintonEcommerce.Application.Abstraction.Messaging;

namespace BadmintonEcommerce.Application.Features.Order.AddToCart;

public sealed class AddToCartCommand : ICommand<Guid>
{
    public Guid VariantId { get; set; }
    public int Quantity { get; set; }
}