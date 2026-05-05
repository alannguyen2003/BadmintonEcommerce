namespace BadmintonEcommerce.Contracts.Endpoints;

public static class ProductCategoryEndpoint
{
    public static readonly string EndpointUrl = "/client/categories";
    public static string GetCategories()
    {
        return $"{EndpointUrl}";
    }

    public static string GetChildCategories(Guid categoryId)
    {
        return $"{EndpointUrl}/child/{categoryId}";
    }
}