using CloudinaryDotNet.Actions;
using SharedKernel.Services;

namespace BadmintonEcommerce.Application.Abstraction.Services;

public interface IFileService
{
    public Task<ImageUploadResult> UploadFileAsync(FileUploadStream file);
}