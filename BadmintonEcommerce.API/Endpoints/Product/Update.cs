using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Product.Update;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;
using BadmintonEcommerce.Mapper.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Product;

public class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("products", async (
            [FromBody] UpdateProductRequest request,
            [FromServices] ICommandHandler<UpdateProductCommand> handler,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken) =>
        {
            var command = mapper.Map<UpdateProductCommand>(request);
            Result result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.NoContent, CustomResult.Problem);
        });
    }
}