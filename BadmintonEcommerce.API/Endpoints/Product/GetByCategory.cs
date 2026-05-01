using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Product.GetByCategory;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Product;

public class GetByCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products/category/{categoryId:guid}", async (
            Guid categoryId,
            IQueryHandler<GetProductsByCategoryQuery, List<ProductResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            Result<List<ProductResponse>> result = await handler.Handle(new GetProductsByCategoryQuery(categoryId), cancellationToken);

            return result.Match(Results.Ok, CustomResult.Problem);
        }).WithTags(Tags.Product);
    }
}