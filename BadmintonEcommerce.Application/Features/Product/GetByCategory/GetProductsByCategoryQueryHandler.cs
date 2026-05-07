using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.Product.GetByCategory;

public class GetProductsByCategoryQueryHandler(
    IProductRepository productRepository,
    IProductCategoryRepository productCategoryRepository,
    IMapper mapper)
    : IQueryHandler<GetProductsByCategoryQuery, List<ProductResponse>>
{
    public async Task<Result<List<ProductResponse>>> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        Domain.Entities.Catalog.ProductCategory? productCategory = productCategoryRepository.GetById(query.ProductCategoryId);

        if (productCategory == null)
            return Result.Failure<List<ProductResponse>>(ProductCategoryError.NotFound(query.ProductCategoryId));

        IEnumerable<Domain.Entities.Catalog.Product> products = await productRepository.Get(
            filter: filter => filter.CategoryId == query.ProductCategoryId,
            null,
            "Category,Variants");

        List<Domain.Entities.Catalog.Product> productsResponse = products.ToList();
        List<ProductResponse> responses = mapper.Map<List<ProductResponse>>(productsResponse);
        
        return responses;
    }
}