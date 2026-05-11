using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Domain.Entities.Catalog;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Entities;

public class ProductBuilder
{
    private Guid _id;
    private string _name;
    private string _description;
    private string _brand;
    private Guid _categoryId;

    public ProductBuilder WithId(Guid id)
    {
        this._id = id;
        return this;
    }

    public ProductBuilder WithName(string name)
    {
        this._name = name;
        return this;
    }

    public ProductBuilder WithDescription(string description)
    {
        this._description = description;
        return this;
    }

    public ProductBuilder WithBrand(string brand)
    {
        this._brand = brand;
        return this;
    }

    public ProductBuilder WithCategoryId(Guid categoryId)
    {
        this._categoryId = categoryId;
        return this;
    }

    public Product Build() => new Product()
    {
        Id = this._id,
        Name = this._name,
        Description = this._description,
        Brand = this._brand,
        CategoryId = this._categoryId
    };

    public ProductResponse ResponseBuild() => new ProductResponse()
    {
        Id = this._id,
        ProductName = this._name
    };
}