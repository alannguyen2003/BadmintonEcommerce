using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.Product.Get;

public class GetProductsQueryHandler(
    IProductRepository productRepository,
    IProductVariantRepository productVariantRepository,
    IMapper mapper) : IQueryHandler<GetProductsQuery, List<ProductResponse>>
{
    public async Task<Result<List<ProductResponse>>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Catalog.Product> productsQuery = await productRepository.Get(
            filter: null,
            orderBy: null,
            "Variants,Images,Category");

        List<Domain.Entities.Catalog.Product> products = productsQuery.ToList();
        List<ProductResponse> responses = mapper.Map<List<ProductResponse>>(products);

        for (int i = 0; i < products.Count; i++)
        {
            var primaryImage = products[i].Images
                .FirstOrDefault(item => item.IsPrimary);
            if (primaryImage != null)
                responses[i].PrimaryImage = new PrimaryImageResponse()
                {
                    Id = primaryImage.Id,
                    Url = primaryImage.Url
                };
            else responses[i].PrimaryImage = null;
        }
        return responses;
    }
}

