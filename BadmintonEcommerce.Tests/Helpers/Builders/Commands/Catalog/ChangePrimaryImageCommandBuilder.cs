using BadmintonEcommerce.Application.Features.Product.ChangePrimaryImage;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Commands.Catalog;

public class ChangePrimaryImageCommandBuilder
{
    public Guid _productId;
    public Guid _imageId;

    public ChangePrimaryImageCommandBuilder WithProductId(Guid productId)
    {
        this._productId = productId;
        return this;
    }

    public ChangePrimaryImageCommandBuilder WithImageId(Guid imageId)
    {
        this._imageId = imageId;
        return this;
    }

    public ChangePrimaryImageCommand Build() => new ChangePrimaryImageCommand()
    {
        ProductId = this._productId,
        ImageId = this._imageId
    };

    public ChangePrimaryImageCommand Valid() => new ChangePrimaryImageCommand()
    {
        ProductId = Guid.NewGuid(),
        ImageId = Guid.NewGuid(),
    };
}