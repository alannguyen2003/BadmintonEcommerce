using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Contracts.API.Presentation.File.Request;

namespace BadmintonEcommerce.Application.Features.Product.UpdateProduct;

public sealed class UpdateFullProductCommand : ICommand
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public Guid CategoryId { get; set; }
    public bool Status { get; set; }
    public List<Guid> DeletedImages { get; set; }
    public List<FileUploadStreamData> AddedImages { get; set; }
    public List<UpdateOption> UpdatedOptions { get; set; }
    public List<Guid> DeletedOptions { get; set; }
    public List<Guid> DeletedOptionValues { get; set; }
    public List<UpdateOptionValue> UpdatedOptionValues { get; set; }
}

public class UpdateOption
{
    public Guid Id { get; set; }
    public string OptionName { get; set; }
}

public class UpdateOptionValue
{
    public Guid Id { get; set; }
    public Guid OptionId { get; set; }
    public string NewValue { get; set; }
}