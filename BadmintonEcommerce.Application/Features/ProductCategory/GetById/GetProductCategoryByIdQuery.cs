using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Responses;

namespace BadmintonEcommerce.Application.Features.ProductCategory.GetById;

public sealed record GetProductCategoryByIdQuery(Guid ProductCategoryId) 
    : IQuery<ProductCategoryByIdResponse>;
