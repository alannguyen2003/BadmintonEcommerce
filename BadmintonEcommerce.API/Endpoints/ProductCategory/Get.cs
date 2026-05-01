using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.ProductCategory.Get;
using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Responses;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.ProductCategory;

public sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (
            IQueryHandler<GetProductCategoriesQuery, List<ProductCategoryResponse>> handler,
            CancellationToken ct) =>
        {
            Result<List<ProductCategoryResponse>> result = await handler.Handle(new GetProductCategoriesQuery(), ct);

            return result.Match(Results.Ok, CustomResult.Problem);
        }).WithTags(Tags.ProductCategory);
    }
}