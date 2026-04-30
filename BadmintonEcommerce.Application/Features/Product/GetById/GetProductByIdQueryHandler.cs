using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.Product.GetById;

public sealed class GetProductByIdQueryHandler(
    IProductRepository productRepository,
    IMapper mapper
    ) : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        Domain.Entities.Catalog.Product? productQuery = productRepository.GetById(query.ProductId);

        if (productQuery == null)
            return Result.Failure<ProductResponse>(ProductError.NotFound(query.ProductId));
        
        ProductResponse response = mapper.Map<ProductResponse>(productQuery);

        return response;
    }
}