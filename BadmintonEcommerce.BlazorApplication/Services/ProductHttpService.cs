using BadmintonEcommerce.BlazorApplication.Abstraction.Services;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Contracts.Endpoints;

namespace BadmintonEcommerce.BlazorApplication.Services;

public class ProductHttpService: IProductHttpService
{
    private readonly HttpClient client;

    public ProductHttpService(IHttpClientFactory httpClientFactory)
    {
        client = httpClientFactory.CreateClient("api");
    }
    
    public async Task<List<ProductResponse>?> GetProductsByCategory(Guid productCategoryId)
    {
        return await client.GetFromJsonAsync<List<ProductResponse>?>(
            ProductEndpoint.GetProductsByCategory(productCategoryId));
    }
}