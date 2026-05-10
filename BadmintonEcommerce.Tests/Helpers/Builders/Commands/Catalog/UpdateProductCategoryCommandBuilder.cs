using BadmintonEcommerce.Application.Features.ProductCategory.Update;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Commands.Catalog;

public class UpdateProductCategoryCommandBuilder
{
    private Guid id;
    private string categoryName;
    private Guid? parentCategoryId;

    public UpdateProductCategoryCommandBuilder WithId(Guid id)
    {
        this.id = id;
        return this;
    }

    public UpdateProductCategoryCommandBuilder WithCategoryName(string categoryName)
    {
        this.categoryName = categoryName;
        return this;
    }

    public UpdateProductCategoryCommandBuilder WithParentCategoryId(Guid? parentCategoryId)
    {
        this.parentCategoryId = parentCategoryId;
        return this;
    }

    public UpdateProductCategoryCommand Build() => new UpdateProductCategoryCommand()
    {
        Id = id,
        CategoryName = categoryName,
        ParentCategoryId = parentCategoryId
    };

    public UpdateProductCategoryCommand Valid() => new UpdateProductCategoryCommand()
    {
        Id = Guid.NewGuid(),
        CategoryName = "Racquets",
        ParentCategoryId = null
    };
}