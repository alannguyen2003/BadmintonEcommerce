using BadmintonEcommerce.Contracts.API.Presentation.Client.Category;
using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Responses;

namespace BadmintonEcommerce.BlazorApplication.Abstraction.Services;

public interface IProductCategoryHttpService
{
    public Task<List<CategoryResponse>?> GetClientCategories();
    public Task<List<ProductCategoryResponse>?> GetChildProductCategories(Guid parentCategoryId);
}