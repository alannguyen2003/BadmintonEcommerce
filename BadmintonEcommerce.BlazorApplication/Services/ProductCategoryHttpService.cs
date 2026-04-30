using BadmintonEcommerce.BlazorApplication.Abstraction.Services;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Responses;
using BadmintonEcommerce.Contracts.Endpoints;

namespace BadmintonEcommerce.BlazorApplication.Services;

public class ProductCategoryHttpService : IProductCategoryHttpService
{
    private readonly HttpClient client;

    public ProductCategoryHttpService(IHttpClientFactory httpClientFactory)
    {
        client = httpClientFactory.CreateClient("api");
    }
    
    public async Task<List<ProductCategoryResponse>?> GetProductCategories(Guid productCategoryId)
    {
        return await client.GetFromJsonAsync<List<ProductCategoryResponse>>(
            ProductCategoryEndpoint.GetCategories());
    }

    public async Task<List<ProductCategoryResponse>?> GetChildProductCategories(Guid parentCategoryId)
    {
        return await client.GetFromJsonAsync<List<ProductCategoryResponse>>(
            ProductCategoryEndpoint.GetChildCategories(parentCategoryId));
    }
}