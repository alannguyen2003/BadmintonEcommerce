using BadmintonEcommerce.BlazorApplication.Abstraction.Services;
using BadmintonEcommerce.Contracts.API.Presentation;
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

    public async Task<PagedList<List<ProductResponse>>> GetProductsByCategoryAndDefault(PagedRequest<Guid> request)
    {
        var result = await client.PostAsJsonAsync(
            ClientEndpoint.GetProductsByCategoryAndDefault, request);

        var content = await result.Content.ReadFromJsonAsync<PagedList<List<ProductResponse>>>();
        return content;
    }
}