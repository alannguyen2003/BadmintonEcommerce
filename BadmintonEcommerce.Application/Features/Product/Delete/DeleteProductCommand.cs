using BadmintonEcommerce.Application.Abstraction.Messaging;

namespace BadmintonEcommerce.Application.Features.Product.Delete;

public sealed record DeleteProductCommand(Guid ProductId) : ICommand;