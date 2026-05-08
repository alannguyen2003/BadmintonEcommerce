using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Client.GetProductDetail;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Client;

public class GetProductDetail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("client/products/{id:guid}", async (
            [FromRoute] Guid id,
            [FromServices] IQueryHandler<GetProductDetailQuery, ProductDetailResponse> handler,
            CancellationToken cancellationToken) =>
        {
            Result<ProductDetailResponse> result = await handler.Handle(new GetProductDetailQuery(id), cancellationToken);

            return result.Match(Results.Ok, CustomResult.Problem);
        }).WithTags(Tags.Client);
    }
}