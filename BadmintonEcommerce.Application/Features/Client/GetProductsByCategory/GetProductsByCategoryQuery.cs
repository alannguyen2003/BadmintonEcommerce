using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Contracts.API.Presentation;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;

namespace BadmintonEcommerce.Application.Features.Client.GetProductsByCategory;

public sealed record GetProductsByCategoryQuery(int PageNumber, int PageSize, Guid CategoryId) 
    : IQuery<PagedList<List<ProductResponse>>>;