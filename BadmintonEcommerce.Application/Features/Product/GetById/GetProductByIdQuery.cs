using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;

namespace BadmintonEcommerce.Application.Features.Product.GetById;

public sealed record GetProductByIdQuery(Guid ProductId) : IQuery<ProductDetailResponse>;