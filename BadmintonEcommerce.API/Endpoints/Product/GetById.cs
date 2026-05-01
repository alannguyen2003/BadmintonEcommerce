using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Product.GetById;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Product;

public class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async (
            Guid id,
            IQueryHandler<GetProductByIdQuery, ProductResponse> handler,
            CancellationToken cancellationToken
        ) =>
        {
            Result<ProductResponse> result = await handler.Handle(new GetProductByIdQuery(id), cancellationToken);

            return result.Match(Results.Ok, CustomResult.Problem);
        }).WithTags(Tags.Product);
    }
}