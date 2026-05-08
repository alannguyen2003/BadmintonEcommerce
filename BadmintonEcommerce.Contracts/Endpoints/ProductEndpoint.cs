namespace BadmintonEcommerce.Contracts.Endpoints;

public static class ProductEndpoint
{
    public static readonly string EndpointUrl = "/products";
    public static string GetProductsByCategory(Guid categoryId)
    {
        return $"{EndpointUrl}/category/{categoryId}";
    }

    public static string GetProduct(Guid productId)
    {
        return $"{EndpointUrl}/{productId}";
    }
    
}