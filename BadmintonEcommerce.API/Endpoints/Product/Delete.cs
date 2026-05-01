using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Product.Delete;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Product;

public class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("products/{id:guid}", async (
            Guid id,
            ICommandHandler<DeleteProductCommand> handler,
            CancellationToken cancellationToken
            ) =>
        {
            Result result = await handler.Handle(new DeleteProductCommand(id), cancellationToken);

            return result.Match(Results.NoContent, CustomResult.Problem);
        }).WithTags(Tags.Product);
    }
}