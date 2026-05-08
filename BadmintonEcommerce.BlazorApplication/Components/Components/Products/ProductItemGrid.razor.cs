using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using Microsoft.AspNetCore.Components;

namespace BadmintonEcommerce.BlazorApplication.Components.Components.Products;

public partial class ProductItemGrid : ComponentBase
{
    [Parameter]
    public ProductResponse Product { get; set; }
}