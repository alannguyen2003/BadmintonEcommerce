using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;

namespace BadmintonEcommerce.Application.Features.Product.GetByCategory;

public sealed record GetProductsByCategoryQuery(Guid ProductCategoryId) : IQuery<List<ProductResponse>>;