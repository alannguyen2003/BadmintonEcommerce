using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.ProductCategory.Update;
using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Requests;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.ProductCategory;

public class Update : IEndpoint
{
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("categories", async (
            [FromBody] UpdateProductCategoryRequest request,
            [FromServices] ICommandHandler<UpdateProductCategoryCommand> handler,
            CancellationToken cancellationToken
            ) =>
        {
            var command = new UpdateProductCategoryCommand()
            {
                Id = request.Id,
                CategoryName = request.CategoryName,
                ParentCategoryId = request.ParentCategoryId
            };
            Result result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.NoContent, CustomResult.Problem);
        }).WithTags(Tags.ProductCategory);
    }
}