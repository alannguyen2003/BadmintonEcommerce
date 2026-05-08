using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Client.GetCategories;
using BadmintonEcommerce.Contracts.API.Presentation.Client.Category;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Client;

public class GetCategories : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("client/categories", async (
            [FromServices] IQueryHandler<GetCategoriesQuery, List<CategoryResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            Result<List<CategoryResponse>> result = await handler.Handle(new GetCategoriesQuery(), cancellationToken);

            return result.Match(Results.Ok, CustomResult.Problem);
        }).WithTags(Tags.Client);
    }
}