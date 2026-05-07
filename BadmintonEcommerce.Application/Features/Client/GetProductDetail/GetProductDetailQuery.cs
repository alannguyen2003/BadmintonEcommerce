using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;

namespace BadmintonEcommerce.Application.Features.Client.GetProductDetail;

public sealed record GetProductDetailQuery(Guid ProductId) : IQuery<ProductDetailResponse>;