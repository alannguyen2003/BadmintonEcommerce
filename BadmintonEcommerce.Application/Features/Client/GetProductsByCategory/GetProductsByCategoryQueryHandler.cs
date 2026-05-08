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
    public async Task<Result<PagedList<List<ProductResponse>>>> Handle(GetProductsByCategoryQuery query,
        CancellationToken cancellationToken)
    {
        //Check if category exists
        Domain.Entities.Catalog.ProductCategory? category = productCategoryRepository
            .GetById(query.CategoryId);
        /*if (category is null)
            return Result.Failure<PagedList<List<ProductResponse>>>(ProductCategoryError.NotFound(query.CategoryId));*/
        List<Domain.Entities.Catalog.Product> products = new List<Domain.Entities.Catalog.Product>();
        if (category is not null)
        {
            IEnumerable<Domain.Entities.Catalog.ProductCategory> categoriesQuery = await productCategoryRepository
                .Get(filter: filter => filter.Id.Equals(query.CategoryId),
                    orderBy: null,
                    includeProperties: "ChildCategories");
            foreach (var item in categoriesQuery.First().ChildCategories)
            {
                IEnumerable<Domain.Entities.Catalog.Product> productsQuery = await productRepository
                    .Get(filter: filter => filter.CategoryId.Equals(item.Id),
                        orderBy: null,
                        "Images,Category,Variants");
                products.AddRange(productsQuery.ToList());
            }

            IEnumerable<Domain.Entities.Catalog.Product> parentCategoryProducts = await productRepository
                .Get(filter: filter => filter.CategoryId.Equals(query.CategoryId),
                    orderBy: null,
                    includeProperties: "Images,Category,Variants");
            
            products.AddRange(parentCategoryProducts.ToList());
            /*IEnumerable<Domain.Entities.Catalog.Product> productsQuery = await productRepository.Get(
                filter: filter => filter.CategoryId.Equals(query.CategoryId),
                orderBy: null,
                includeProperties: "Images");*/
        }
        else
        {
            IEnumerable<Domain.Entities.Catalog.Product> defaultCategoryProducts = await productRepository
                .Get(filter: null,
                    orderBy: null,
                    includeProperties: "Images,Category,Variants");
            products.AddRange(defaultCategoryProducts.ToList());
        }

        int totalCount = products.Count();
        int validPageIndex = query.PageNumber > 0 ? query.PageNumber - 1 : 0;
        int validPageSize = query.PageSize > 0 ? query.PageSize : 9;
        products = products
            .Skip(validPageIndex * validPageSize)
            .Take(validPageSize).ToList();

        return new PagedList<List<ProductResponse>>()
        {
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            Data = mapper.Map<List<ProductResponse>>(products),
            TotalCount = totalCount,
            TotalPages = (int) Math.Ceiling(totalCount / (double) validPageSize),
        };
    }
}