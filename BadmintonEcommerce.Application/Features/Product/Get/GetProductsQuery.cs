using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;

namespace BadmintonEcommerce.Application.Features.Product.Get;

public sealed record GetProductsQuery : IQuery<List<ProductResponse>>;