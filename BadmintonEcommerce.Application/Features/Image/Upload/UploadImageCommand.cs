
using BadmintonEcommerce.Application.Abstraction.Messaging;
using SharedKernel.Services;

namespace BadmintonEcommerce.Application.Features.Image.Upload;

public sealed class UploadImageCommand: ICommand<string>
{
    public FileUploadStream FileUpload { get; set; }
}