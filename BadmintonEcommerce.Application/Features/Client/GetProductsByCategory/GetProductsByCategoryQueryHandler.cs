using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Contracts.API.Presentation;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.Client.GetProductsByCategory;

public class GetProductsByCategoryQueryHandler(
    IProductCategoryRepository productCategoryRepository,
    IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<GetProductsByCategoryQuery, PagedList<List<ProductResponse>>>
{
    public async Task<Result<PagedList<List<ProductResponse>>>> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        //Check if category exists
        Domain.Entities.Catalog.ProductCategory? category = productCategoryRepository
            .GetById(query.CategoryId);
        if (category is null)
            return Result.Failure<PagedList<List<ProductResponse>>>(ProductCategoryError.NotFound(query.CategoryId));

        IEnumerable<Domain.Entities.Catalog.ProductCategory> categoriesQuery = await productCategoryRepository
            .Get(filter: filter => filter.Id.Equals(query.CategoryId),
                orderBy: null,
                includeProperties: "ChildCategories");
        List<Domain.Entities.Catalog.Product> products = new List<Domain.Entities.Catalog.Product>();
        Console.WriteLine(categoriesQuery.First().ChildCategories.Count);
        foreach (var item in categoriesQuery.First().ChildCategories)
        {
            IEnumerable<Domain.Entities.Catalog.Product> productsQuery = await productRepository
                .Get(filter: filter => filter.CategoryId.Equals(item.Id),
                    orderBy: null,
                    "Images,Category,Variants");
            products.AddRange(productsQuery);
        }
        /*IEnumerable<Domain.Entities.Catalog.Product> productsQuery = await productRepository.Get(
            filter: filter => filter.CategoryId.Equals(query.CategoryId),
            orderBy: null,
            includeProperties: "Images");*/

        return new PagedList<List<ProductResponse>>()
        {
            PageNumber = 1,
            PageSize = 1,
            Data = mapper.Map<List<ProductResponse>>(products)
        };
    }
}