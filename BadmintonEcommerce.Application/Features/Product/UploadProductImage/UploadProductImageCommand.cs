using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Contracts.API.Presentation.File.Request;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;

namespace BadmintonEcommerce.Application.Features.Product.UploadProductImage;

public sealed class UploadProductImageCommand : ICommand<List<ProductImageResponse>>
{
    public Guid ProductId { get; set; }
    public List<FileUploadStreamData> Files { get; set; }
}