namespace BadmintonEcommerce.API.Endpoints.Client;

public class GetProducts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("client/products", async () =>
        {
            
        }).WithTags(Tags.Client);
    }
}