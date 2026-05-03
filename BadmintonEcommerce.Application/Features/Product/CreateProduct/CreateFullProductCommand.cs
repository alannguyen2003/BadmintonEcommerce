using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Contracts.API.Presentation.File.Request;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;

namespace BadmintonEcommerce.Application.Features.Product.CreateProduct;

public class CreateFullProductCommand : ICommand<Guid>
{
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public string Brand { get; set; }
    public Guid ProductCategoryId { get; set; }
    public bool Status { get; set; }
    public List<CreateOptionRequest> OptionRequests { get; set; }
    public List<CreateVariantRequest> VariantRequests { get; set; }
    public List<FileUploadStreamData> Files { get; set; }
}