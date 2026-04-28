using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.ProductCategory.GetById;
using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Responses;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.ProductCategory;

public class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories/{id:guid}", async (
            Guid id,
            IQueryHandler<GetProductCategoryByIdQuery, ProductCategoryByIdResponse> handler,
            CancellationToken cancellationToken) =>
        {
            Result<ProductCategoryByIdResponse> result = await handler.Handle(new GetProductCategoryByIdQuery(id), cancellationToken);
            
            return result.Match(Results.Ok, CustomResult.Problem);
        });
    }
}