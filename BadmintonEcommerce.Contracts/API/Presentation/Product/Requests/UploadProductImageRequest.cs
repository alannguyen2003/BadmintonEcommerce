using BadmintonEcommerce.Contracts.API.Presentation.File.Request;

namespace BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;

public class UploadProductImageRequest
{
    public Guid ProductId { get; set; }
    public List<FileUploadStreamData> FileDatas { get; set; }
}