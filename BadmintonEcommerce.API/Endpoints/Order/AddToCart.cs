namespace BadmintonEcommerce.API.Endpoints.Order;

public class AddToCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("cart/add-to-cart", async () =>
        {
            
        }).RequireAuthorization();
    }
}