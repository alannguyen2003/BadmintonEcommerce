using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Responses;
using BadmintonEcommerce.Domain.Entities.Catalog;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Entities;

public class ProductCategoryBuilder
{
    private Guid id;
    private string name;

    public ProductCategoryBuilder WithId(Guid id)
    {
        this.id = id;
        return this;
    }

    public ProductCategoryBuilder WithName(string name)
    {
        this.name = name;
        return this;
    }

    public ProductCategory Build() => new ProductCategory()
    {
        Id = this.id,
        CategoryName = this.name
    };

    public ProductCategoryResponse ResponseBuild() => new ProductCategoryResponse()
    {
        CategoryName = this.name,
        Id = this.id
    };
}