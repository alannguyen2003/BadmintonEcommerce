using BadmintonEcommerce.Domain.Entities.Catalog;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Entities;

public class ProductImageBuilder
{
    private Guid _id;
    private Guid _productId;
    private bool _isPrimary;

    public ProductImageBuilder WithId(Guid id)
    {
        this._id = id;
        return this;
    }

    public ProductImageBuilder WithProductId(Guid productId)
    {
        this._productId = productId;
        return this;
    }

    public ProductImageBuilder WithIsPrimary(bool isPrimary)
    {
        this._isPrimary = isPrimary;
        return this;
    }

    public ProductImage Build() => new ProductImage()
    {
        Id = this._id,
        ProductId = this._productId,
        IsPrimary = this._isPrimary
    };
}