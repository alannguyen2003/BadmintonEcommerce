using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.Product.Get;

public class GetProductQueryHandler(IProductRepository productRepository)
    : IQueryHandler<GetProductQuery, List<ProductResponse>>
{
    public Task<Result<List<ProductResponse>>> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        return null;
    }
}