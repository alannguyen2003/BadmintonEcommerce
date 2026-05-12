using BadmintonEcommerce.Application.Features.Product.GetByCategory;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Queries;

public class GetProductsByCategoryIdQueryBuilder
{
    private Guid _productCategoryId;

    public GetProductsByCategoryIdQueryBuilder WithProductCategoryId(Guid productCategoryId)
    {
        this._productCategoryId = productCategoryId;
        return this;
    }

    public GetProductsByCategoryQuery Build() 
        => new GetProductsByCategoryQuery(this._productCategoryId);

}