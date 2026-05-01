using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Product.Create;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;
using BadmintonEcommerce.Infrastructure.Persistence.Profiles;
using BadmintonEcommerce.Mapper.Abstractions;
using BadmintonEcommerce.Mapper.Configurations;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Product;

public class Create : IEndpoint
{
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("products", async (
            [FromBody] CreateProductRequest request,
            [FromServices] IMapper mapper, 
            [FromServices] ICommandHandler<CreateProductCommand, Guid> handler,
            CancellationToken cancellationToken
        ) =>
        {
            CreateProductCommand command = mapper.Map<CreateProductCommand>(request);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Created, CustomResult.Problem);
        }).WithTags(Tags.Product);
    }
}