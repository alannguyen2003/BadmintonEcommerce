using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Image.Upload;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.API.Endpoints.Image;

public class Create : IEndpoint
{
    public class UploadRequest 
    {
        public IFormFileCollection Files { get; set; }
    }
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("image", async (
            [FromForm] UploadRequest request, 
            [FromServices] ICommandHandler<UploadImageCommand, string> handler, 
            CancellationToken cancellationToken) =>
        {
            Result<UploadRequest> demo = Result.Success(request);
            return demo.Match(Results.Ok, CustomResult.Problem);
            /*var command = new UploadImageCommand()
            {
                FileUpload = new FileUploadStream()
                {
                    FileName = request.File.FileName,
                    ContentType = request.File.ContentType,
                    Stream = request.File.OpenReadStream()
                }
            };

            Result<string> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResult.Problem);*/
        }).DisableAntiforgery();
    }
}