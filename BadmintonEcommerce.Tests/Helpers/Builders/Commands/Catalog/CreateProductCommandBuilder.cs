using BadmintonEcommerce.Application.Features.Product.Create;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Commands.Catalog;

public class CreateProductCommandBuilder
{
    private string _productName;
    private string _description;
    private string _brand;
    private Guid _categoryId;

    public CreateProductCommandBuilder WithProductName(string productName)
    {
        this._productName = productName;
        return this;
    }

    public CreateProductCommandBuilder WithDescription(string description)
    {
        this._description = description;
        return this;
    }

    public CreateProductCommandBuilder WithBrand(string brand)
    {
        this._brand = brand;
        return this;
    }

    public CreateProductCommandBuilder WithCategoryId(Guid categoryId)
    {
        this._categoryId = categoryId;
        return this;
    }

    public CreateProductCommand Build() => new CreateProductCommand()
    {
        ProductName = this._productName,
        Description = this._description,
        Brand = this._brand,
        CategoryId = this._categoryId
    };

    public CreateProductCommand Valid() => new CreateProductCommand()
    {
        ProductName = "Racquets",
        Description = "Vot cua Nguyen Ho",
        Brand = "Yonex",
        CategoryId = Guid.NewGuid()
    };
}