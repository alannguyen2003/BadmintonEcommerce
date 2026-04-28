using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.ProductCategory.Update;
using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Requests;
using Microsoft.AspNetCore.Mvc;

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
            await handler.Handle(command, cancellationToken);
            return;
        });
    }
}