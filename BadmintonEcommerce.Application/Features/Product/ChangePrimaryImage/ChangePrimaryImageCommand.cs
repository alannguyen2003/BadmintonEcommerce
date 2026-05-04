using BadmintonEcommerce.Application.Abstraction.Messaging;

namespace BadmintonEcommerce.Application.Features.Product.ChangePrimaryImage;

public sealed class ChangePrimaryImageCommand : ICommand
{
    public Guid ProductId { get; set; }
    public Guid ImageId { get; set; }
}