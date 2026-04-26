using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.ProductCategory.Get;

public class GetProductCategoriesQueryHandler(IMapper mapper, IProductCategoryRepository productCategoryRepository) : 
    IQueryHandler<GetProductCategoriesQuery, List<ProductCategoryResponse>>
{
    public async Task<Result<List<ProductCategoryResponse>>> Handle(GetProductCategoriesQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Catalog.ProductCategory> categories =
            await productCategoryRepository.Get(null, null, "ParentCategory");
        
        List<Domain.Entities.Catalog.ProductCategory> productCategoriesRaw = categories.ToList();
        
        List<ProductCategoryResponse> responses = mapper.Map<List<ProductCategoryResponse>>(productCategoriesRaw);
        return responses;
    }
}