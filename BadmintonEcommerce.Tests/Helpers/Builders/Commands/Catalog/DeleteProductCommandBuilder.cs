using BadmintonEcommerce.Application.Features.Product.Delete;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Commands.Catalog;

public class DeleteProductCommandBuilder
{
    private Guid _productId;

    public DeleteProductCommandBuilder WithProductId(Guid productId)
    {
        this._productId = productId;
        return this;
    }
    
    public DeleteProductCommand Build() => new DeleteProductCommand(this._productId);
}