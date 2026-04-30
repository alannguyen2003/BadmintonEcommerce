using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.Product.Get;

public class GetProductsQueryHandler(
    IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<GetProductsQuery, List<ProductResponse>>
{
    public async Task<Result<List<ProductResponse>>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Catalog.Product> productsQuery = await productRepository.Get();

        List<Domain.Entities.Catalog.Product> products = productsQuery.ToList();

        return mapper.Map<List<ProductResponse>>(products);
    }
}