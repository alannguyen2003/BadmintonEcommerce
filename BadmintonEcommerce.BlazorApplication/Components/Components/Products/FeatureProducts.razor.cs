using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using Microsoft.AspNetCore.Components;

namespace BadmintonEcommerce.BlazorApplication.Components.Components.Products;

public partial class FeatureProducts : ComponentBase
{
    [Parameter]
    public List<ProductResponse> ProductResponses { get; set; }
}