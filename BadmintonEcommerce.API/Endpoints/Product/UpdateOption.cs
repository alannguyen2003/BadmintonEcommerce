using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Product.UpdateOption;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Product;

public class UpdateOption : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("products/update-options", async (
            [FromBody] UpdateOptionRequest request,
            [FromServices] ICommandHandler<UpdateOptionCommand> handler,
            CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(new UpdateOptionCommand()
            {
                AddedOptions = request.AddedOptions,
                AddedVariants = request.AddedVariants,
                DeletedVariants = request.DeletedVariants,
                DeletedOptionValues = request.DeletedOptionValues,
                DeletedOptions = request.DeletedOptions,
                ProductId = request.ProductId
            }, cancellationToken);
            return result.Match(Results.NoContent, CustomResult.Problem);
        });
    }
}