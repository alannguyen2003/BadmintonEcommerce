
using System.Diagnostics.CodeAnalysis;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using SharedKernel.Services;

namespace BadmintonEcommerce.Application.Features.Image.Upload;
[ExcludeFromCodeCoverage]
public sealed class UploadImageCommand: ICommand<string>
{
    public FileUploadStream FileUpload { get; set; }
}