using BadmintonEcommerce.Application.Abstraction.Messaging;

namespace BadmintonEcommerce.Application.Features.ProductCategory.Get;

public sealed record GetProductCategoriesQuery() : IQuery<List<Domain.Entities.Catalog.ProductCategory>>;