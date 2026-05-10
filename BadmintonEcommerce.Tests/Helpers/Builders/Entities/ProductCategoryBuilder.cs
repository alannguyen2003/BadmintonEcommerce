using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Responses;
using BadmintonEcommerce.Domain.Entities.Catalog;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Entities;

public class ProductCategoryBuilder
{
    private Guid id;
    private string name;
    private int level;
    private Guid? parentCategoryId;

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

    public ProductCategoryBuilder WithLevel(int level)
    {
        this.level = level;
        return this;
    }

    public ProductCategoryBuilder WithParentCategoryId(Guid? parentCategoryId)
    {
        this.parentCategoryId = parentCategoryId;
        return this;
    }

    public ProductCategory Build() => new ProductCategory()
    {
        Id = this.id,
        CategoryName = this.name,
        Level = this.level,
        ParentCategoryId = this.parentCategoryId
    };

    public ProductCategoryResponse ResponseBuild() => new ProductCategoryResponse()
    {
        CategoryName = this.name,
        Id = this.id
    };
}