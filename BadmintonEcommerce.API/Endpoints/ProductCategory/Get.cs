using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.ProductCategory.Get;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.ProductCategory;

public sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (
            IQueryHandler<GetProductCategoriesQuery, List<Domain.Entities.Catalog.ProductCategory>> handler,
            CancellationToken ct) =>
        {
            var query = new GetProductCategoriesQuery();
            Result<List<Domain.Entities.Catalog.ProductCategory>> result = await handler.Handle(query, ct);

            return result.Match(Results.Ok, CustomResult.Problem);
        });
    }
}