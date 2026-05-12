using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;

namespace BadmintonEcommerce.Application.Features.Product.UpdateOption;

public sealed class UpdateOptionCommand : ICommand
{
    public Guid ProductId { get; set; }
    public List<CreateOptionRequest> AddedOptions { get; set; } = new List<CreateOptionRequest>();
    public List<CreateVariantRequest> AddedVariants { get; set; } = new List<CreateVariantRequest>();
    
    public List<Guid> DeletedOptions { get; set; }
    public List<Guid> DeletedVariants { get; set; }
}

