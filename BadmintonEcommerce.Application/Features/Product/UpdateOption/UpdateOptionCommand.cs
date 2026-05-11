using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;

namespace BadmintonEcommerce.Application.Features.Product.UpdateOption;

public sealed class UpdateOptionCommand : ICommand
{
    public Guid ProductId { get; set; }
    public List<AddedOptionRequest> AddedOptions { get; set; } = new List<AddedOptionRequest>();
    public List<AddedVariantRequest> AddedVariants { get; set; } = new List<AddedVariantRequest>();
    
    public List<Guid> DeletedOptions { get; set; }
    public List<Guid> DeletedOptionValues { get; set; }
    public List<Guid> DeletedVariants { get; set; }
}

