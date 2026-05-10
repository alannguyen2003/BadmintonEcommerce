using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Domain.Entities.Catalog;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Entities;

public class ProductBuilder
{
    private Guid id;
    private string name;

    public ProductBuilder WithId(Guid id)
    {
        this.id = id;
        return this;
    }

    public ProductBuilder WithName(string name)
    {
        this.name = name;
        return this;
    }

    public Product Build() => new Product()
    {
        Id = this.id,
        Name = this.name
    };

    public ProductResponse ResponseBuild() => new ProductResponse()
    {
        Id = this.id,
        ProductName = this.name
    };
}