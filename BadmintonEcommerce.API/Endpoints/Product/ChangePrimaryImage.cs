using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Product.ChangePrimaryImage;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Product;

public class ChangePrimaryImage : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("products/change-primary-image", async (
            [FromBody] ChangePrimaryImageRequest request,
            [FromServices] ICommandHandler<ChangePrimaryImageCommand> handler,
            CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(new ChangePrimaryImageCommand()
            {
                ImageId = request.ImageId,
                ProductId = request.ProductId
            }, cancellationToken);
            return result.Match(Results.NoContent, CustomResult.Problem);
        }).WithTags(Tags.Product);
    }
}