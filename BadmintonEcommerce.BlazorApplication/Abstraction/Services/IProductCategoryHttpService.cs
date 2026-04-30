using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Responses;

namespace BadmintonEcommerce.BlazorApplication.Abstraction.Services;

public interface IProductCategoryHttpService
{
    public Task<List<ProductCategoryResponse>?> GetProductCategories(Guid productCategoryId);
    public Task<List<ProductCategoryResponse>?> GetChildProductCategories(Guid parentCategoryId);
}