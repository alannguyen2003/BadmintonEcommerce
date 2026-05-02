using BadmintonEcommerce.Application.Abstraction.Messaging;

namespace BadmintonEcommerce.Application.Features.Product.CreateOption;

public class CreateProductOptionValueCommand : ICommand
{
    public Guid ProductId { get; set; }
    public string OptionName { get; set; }
    public string[] OptionValues { get; set; }
}