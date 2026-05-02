using BadmintonEcommerce.Application.Abstraction.Messaging;

namespace BadmintonEcommerce.Application.Features.Product.DeleteOption;

public sealed record DeleteOptionCommand(Guid ProductOptionId) : ICommand;