using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Product.UpdateProduct;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Product;

public class UpdateProduct : IEndpoint
{
    public sealed class UpdateFullProductRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public Guid CategoryId { get; set; }
        public string DeletedImages { get; set; }
        public string DeletedOptions { get; set; }
        public string DeletedOptionValues { get; set; }
    }
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("products/update-product", async (
            [FromForm] UpdateFullProductRequest request,
            [FromServices] ICommandHandler<UpdateFullProductCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateFullProductCommand();
            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResult.Problem);
        });
    }
}