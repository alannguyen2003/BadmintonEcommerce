using System.Text.Json;
using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Product.CreateProduct;
using BadmintonEcommerce.Contracts.API.Presentation.File.Request;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Product;

public class CreateProduct : IEndpoint
{
    public class CreateProductFullRequest
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Options { get; set; }
        public string SkuRows { get; set; }
        public IFormFileCollection Images { get; set; }
    }

    public class Option
    {
        public string Name { get; set; }
        public string[] Values { get; set; }
    }
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("products/create-products", async (
            [FromForm] CreateProductFullRequest request,
            [FromServices] ICommandHandler<CreateFullProductCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            List<CreateOptionRequest>? options = JsonSerializer.Deserialize<List<CreateOptionRequest>>(request.Options,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            CreateFullProductCommand command = new CreateFullProductCommand()
            {
                ProductName = request.Name,
                Brand = request.Brand,
                ProductCategoryId = request.CategoryId,
                Status = request.Status,
                ProductDescription = request.Description,
                Files = new List<FileUploadStreamData>(),
                OptionRequests = new List<CreateOptionRequest>(),
                VariantRequests = new List<CreateVariantRequest>()
            };

            foreach (var file in request.Images)
            {
                command.Files.Add(new FileUploadStreamData()
                {
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    Stream = file.OpenReadStream()
                });
            }

            command.OptionRequests = options;
            //Result<Guid> result = await handler.Handle(command, cancellationToken);
            //Result<Guid> result = await handler.Handle(command, cancellationToken);
            List<CreateVariantRequest> variants =
                JsonSerializer.Deserialize<List<CreateVariantRequest>>(request.SkuRows, 
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
            command.VariantRequests = variants;
            Result<Guid> result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.Ok, CustomResult.Problem);
        }).DisableAntiforgery();
    }
}