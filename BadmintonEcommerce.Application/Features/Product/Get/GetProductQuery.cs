using BadmintonEcommerce.Application.Abstraction.Messaging;

namespace BadmintonEcommerce.Application.Features.Product.Get;

public sealed record GetProductQuery(Guid ProductCategoryId) : IQuery<List<ProductResponse>>;