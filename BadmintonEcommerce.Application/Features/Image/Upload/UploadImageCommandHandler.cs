using System.Diagnostics.CodeAnalysis;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Services;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Application.Features.Image.Upload;

[ExcludeFromCodeCoverage]
public sealed class UploadImageCommandHandler(IFileService fileService) : 
    ICommandHandler<UploadImageCommand, string>
{
    public async Task<Result<string>> Handle(UploadImageCommand command, CancellationToken cancellationToken)
    {
        var result = await fileService.UploadFileAsync(command.FileUpload);
        return result.SecureUrl.AbsoluteUri;
    }
}