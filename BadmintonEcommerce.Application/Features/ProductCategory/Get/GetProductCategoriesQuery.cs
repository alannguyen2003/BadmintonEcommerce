using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Responses;

namespace BadmintonEcommerce.Application.Features.ProductCategory.Get;

public sealed record GetProductCategoriesQuery() : IQuery<List<ProductCategoryResponse>>;