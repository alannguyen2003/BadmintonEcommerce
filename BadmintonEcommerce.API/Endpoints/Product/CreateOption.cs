using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Product.CreateOption;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Product;

public class CreateOption : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("products/option", async (
            [FromBody] CreateProductOptionRequest request,
            [FromServices] ICommandHandler<CreateProductOptionValueCommand> handler,
            CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(new CreateProductOptionValueCommand()
            {
                ProductId = request.ProductId,
                OptionName = request.OptionName,
                OptionValues = request.OptionValues
            }, cancellationToken);
            return result.Match(Results.Created, CustomResult.Problem);
        });
    }
}