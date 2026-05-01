using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.ProductCategory.Create;
using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Requests;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.ProductCategory;

public sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("categories", async (
            CreateProductCategoryRequest request,
            ICommandHandler<CreateProductCategoryCommand, Guid> handler, 
            CancellationToken cancellationToken) =>
        {
            var command = new CreateProductCategoryCommand()
            {
                CategoryName = request.CategoryName,
                ParentCategoryId = request.ParantCategoryId != null ? request.ParantCategoryId : null
            };

            Result<Guid> result = await handler.Handle(command, cancellationToken);
            
            return result.Match(Results.Created, CustomResult.Problem);
        }).WithTags(Tags.ProductCategory);
    }
}