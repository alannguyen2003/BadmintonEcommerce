using BadmintonEcommerce.Contracts.API.Presentation;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;

namespace BadmintonEcommerce.BlazorApplication.Abstraction.Services;

public interface IProductHttpService
{
    public Task<List<ProductResponse>?> GetProductsByCategory(Guid productCategoryId);
    public Task<PagedList<List<ProductResponse>>> GetProductsByCategoryAndDefault(PagedRequest<Guid> request);
}