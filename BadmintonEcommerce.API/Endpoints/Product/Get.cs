using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Product.Get;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Product;

public class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (
            IQueryHandler<GetProductsQuery, List<ProductResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            Result<List<ProductResponse>> result = await handler.Handle(new GetProductsQuery(), cancellationToken);
            return result.Match(Results.Ok, CustomResult.Problem);
        }).WithTags(Tags.Product);
    }
}