using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Client;

public class GetProductsByCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("client/products/{categoryId:guid}", async (
            [FromRoute] Guid categoryId,
            CancellationToken cancellationToken) =>
        {
            Dictionary<string, string> items = new Dictionary<string, string>()
            {
                {
                    "categoryId",
                    "{categoryId}"
                },
                {
                    "categoryId1",
                    "{categoryId}"
                },
                {
                    "categoryId2",
                    "{categoryId}"
                },
                {
                    "categoryId3",
                    "{categoryId}"
                },
                {
                    "categoryId4",
                    "{categoryId}"
                },
            };
            Result result = Result.Success();
            return result.Match(Results.NoContent, CustomResult.Problem);
        });
    }
}