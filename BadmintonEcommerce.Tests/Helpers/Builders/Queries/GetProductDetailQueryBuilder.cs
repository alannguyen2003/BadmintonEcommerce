using BadmintonEcommerce.Application.Features.Client.GetProductDetail;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Queries;

public class GetProductDetailQueryBuilder
{
    private Guid productId;

    public GetProductDetailQueryBuilder WithProductId(Guid product)
    {
        this.productId = product;
        return this;
    }
    
    public GetProductDetailQuery Build() => new GetProductDetailQuery(productId);
}