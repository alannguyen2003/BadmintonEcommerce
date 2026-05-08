using Microsoft.AspNetCore.Components;

namespace BadmintonEcommerce.BlazorApplication.Components.Pages;

public partial class ProductDetailPage : ComponentBase
{
    [Parameter]
    public Guid ProductId { get; set; }
}