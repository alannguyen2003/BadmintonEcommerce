using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.ProductCategory.Delete;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.ProductCategory;

public class Delete : IEndpoint
{
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("categories/{id}", async (
            [FromRoute] Guid id,
            [FromServices] ICommandHandler<DeleteProductCategoryCommand> handler,
            CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(new DeleteProductCategoryCommand(id), cancellationToken);

            return result.Match(Results.NoContent, CustomResult.Problem);
        });
    }
}