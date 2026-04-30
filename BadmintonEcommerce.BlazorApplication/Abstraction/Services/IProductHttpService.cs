using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;

namespace BadmintonEcommerce.BlazorApplication.Abstraction.Services;

public interface IProductHttpService
{
    public Task<List<ProductResponse>?> GetProductsByCategory(Guid productCategoryId);
}