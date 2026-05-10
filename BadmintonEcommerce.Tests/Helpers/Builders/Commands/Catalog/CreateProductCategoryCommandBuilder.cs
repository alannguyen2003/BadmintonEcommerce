using BadmintonEcommerce.Application.Features.ProductCategory.Create;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Commands.Catalog;

public class CreateProductCategoryCommandBuilder
{
    private string name;
    private Guid? parentCategoryId;

    public CreateProductCategoryCommandBuilder WithName(string name)
    {
        this.name = name;
        return this;
    }

    public CreateProductCategoryCommandBuilder WithParentCategoryId(Guid? parentCategoryId)
    {
        this.parentCategoryId = parentCategoryId;
        return this;
    }

    public CreateProductCategoryCommand Build() => new CreateProductCategoryCommand()
    {
        CategoryName = this.name,
        ParentCategoryId = this.parentCategoryId
    };

    public CreateProductCategoryCommand Valid() => new CreateProductCategoryCommand()
    {
        CategoryName = Constants.Catalog.Create.CreateValidCategory.Name,
        ParentCategoryId = Constants.Catalog.Create.CreateValidCategory.ParentCategoryId
    };
}