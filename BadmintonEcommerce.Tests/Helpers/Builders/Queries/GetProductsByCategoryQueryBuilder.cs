using BadmintonEcommerce.Application.Features.Client.GetProductsByCategory;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Tests.Helpers.Builders.Entities;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Queries;

public class GetProductsByCategoryQueryBuilder
{
    private int pageNumber;
    private int pageSize;
    private Guid categoryId;

    public GetProductsByCategoryQueryBuilder WithPageNumber(int pageNumber)
    {
        this.pageNumber = pageNumber;
        return this;
    }

    public GetProductsByCategoryQueryBuilder WithPageSize(int pageSize)
    {
        this.pageSize = pageSize;
        return this;
    }

    public GetProductsByCategoryQueryBuilder WithCategoryId(Guid categoryId)
    {
        this.categoryId = categoryId;
        return this;
    }
    
    public GetProductsByCategoryQuery Build() => 
        new GetProductsByCategoryQuery(pageNumber, pageSize, categoryId);
    
    public static List<Product> CreateProducts(int count) 
        => Enumerable.Range(1, count)
            .Select(item => new ProductBuilder()
                .WithId(Guid.NewGuid())
                .WithName($"Product {item}")
                .Build()).ToList();

    public static List<ProductResponse> CreateMappedResponses(int count)
        => Enumerable.Range(1, count)
            .Select(item => new ProductBuilder()
                .WithName($"Product {item}")
                .ResponseBuild())
            .ToList();
}