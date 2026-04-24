using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.ProductCategory.Get;

public class GetProductCategoriesQueryHandler(IMapper mapper, IProductCategoryRepository productCategoryRepository) : IQueryHandler<GetProductCategoriesQuery, List<Domain.Entities.Catalog.ProductCategory>>
{
    public async Task<Result<List<Domain.Entities.Catalog.ProductCategory>>> Handle(GetProductCategoriesQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Catalog.ProductCategory> categories =
            await productCategoryRepository.Get(null, null, "");
        return categories
            .ToList();
    }
}