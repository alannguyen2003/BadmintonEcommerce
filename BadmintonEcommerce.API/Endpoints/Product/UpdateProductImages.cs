using System.Text.Json;
using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Product.UpdateProductImages;
using BadmintonEcommerce.Contracts.API.Presentation.File.Request;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Product;

public class UpdateProductImages : IEndpoint
{
    public class UpdateProductImagesRequest
    {
        public Guid ProductId { get; set; }
        public string ListImagesDeleted { get; set; }
        public IFormFileCollection AddedImages { get; set; }
    }
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("products/update-images", async (
            [FromForm] UpdateProductImagesRequest request,
            [FromServices] ICommandHandler<UpdateProductImageCommand> handler,
            CancellationToken cancellationToken) =>
        {
            List<Guid> deletedImages = JsonSerializer.Deserialize<List<Guid>>(request.ListImagesDeleted,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Result result = await handler.Handle(new UpdateProductImageCommand()
            {
                ProductId = request.ProductId,
                DeletedImages = deletedImages,
                AddedImages = request.AddedImages.Select(item => new FileUploadStreamData()
                {
                    ContentType = item.ContentType,
                    FileName = item.FileName,
                    Stream = item.OpenReadStream()
                }).ToList()
            }, cancellationToken);
            
            /*
            Result<List<Guid>> result = Result.Success(deletedImages);
            */
            return result.Match(Results.NoContent, CustomResult.Problem);
        }).DisableAntiforgery();
    }
}