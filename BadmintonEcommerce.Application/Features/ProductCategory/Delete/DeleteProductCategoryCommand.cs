using BadmintonEcommerce.Application.Abstraction.Messaging;

namespace BadmintonEcommerce.Application.Features.ProductCategory.Delete;

public sealed record DeleteProductCategoryCommand(Guid ProductCategoryId) : ICommand;