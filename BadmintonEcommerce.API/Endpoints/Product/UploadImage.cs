using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Product.UploadProductImage;
using BadmintonEcommerce.Contracts.API.Presentation.File.Request;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Errors;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Product;

public class UploadImage : IEndpoint
{
    public class UploadImageDataFormRequest
    {
        public Guid ProductId { get; set; }
        public IFormFileCollection FileDatas { get; set; }
    }
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("products/upload-images", async (
            [FromForm] UploadImageDataFormRequest request,
            [FromServices] ICommandHandler<UploadProductImageCommand, List<ProductImageResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UploadProductImageCommand()
            {
                ProductId = request.ProductId,
                Files = new List<FileUploadStreamData>()
            };
            foreach (var item in request.FileDatas)
            {
                command.Files.Add(new FileUploadStreamData()
                {
                    FileName = item.FileName,
                    ContentType = item.ContentType,
                    Stream = item.OpenReadStream()
                });
            }
            Result<List<ProductImageResponse>> result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.Ok, CustomResult.Problem);
        }).DisableAntiforgery()
        .WithTags(Tags.Product);
    }
}