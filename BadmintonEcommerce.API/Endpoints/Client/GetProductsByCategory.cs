using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Client.GetProductsByCategory;
using BadmintonEcommerce.Contracts.API.Presentation;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Client;

public class GetProductsByCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("client/products", async (
            [FromBody] PagedRequest<Guid> request,
            [FromServices] IQueryHandler<GetProductsByCategoryQuery, PagedList<List<ProductResponse>>> handler,
            CancellationToken cancellationToken) =>
        {
            Result<PagedList<List<ProductResponse>>> result = await handler.Handle(new GetProductsByCategoryQuery(
                    PageNumber: request.PageNumber,
                    PageSize: request.PageSize,
                    CategoryId: request.Data), 
                cancellationToken);
            return result.Match(Results.Ok, CustomResult.Problem);
        });
    }
}