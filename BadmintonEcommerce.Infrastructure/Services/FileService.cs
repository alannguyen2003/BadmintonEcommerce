using BadmintonEcommerce.Application.Abstraction.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using SharedKernel.Services;

namespace BadmintonEcommerce.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly Cloudinary _cloudinary;
    
    public FileService(IOptions<CloudinarySetting> options)
    {
        var account = new Account(options.Value.CloudName,
            options.Value.ApiKey,
            options.Value.ApiSecret);
        _cloudinary = new Cloudinary(account);
    }
    public async Task<ImageUploadResult> UploadFileAsync(FileUploadStream file)
    {
        var result = await _cloudinary.UploadAsync(
            new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.Stream),
                DisplayName = file.FileName,
                Folder = "badminton-ecommerce"
            });

        if (result != null && result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return result;
        }

        return null;
    }
}