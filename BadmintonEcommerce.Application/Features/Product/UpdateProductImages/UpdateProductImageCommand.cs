using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Contracts.API.Presentation.File.Request;

namespace BadmintonEcommerce.Application.Features.Product.UpdateProductImages;

public class UpdateProductImageCommand : ICommand
{
    public Guid ProductId { get; set; }
    
    public List<Guid> DeletedImages { get; set; }
    public List<FileUploadStreamData> AddedImages { get; set; }
}