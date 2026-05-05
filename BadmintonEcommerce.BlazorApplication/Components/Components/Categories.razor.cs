using BadmintonEcommerce.BlazorApplication.Abstraction.Services;
using BadmintonEcommerce.Contracts.API.Presentation.Client.Category;
using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Responses;
using Microsoft.AspNetCore.Components;

namespace BadmintonEcommerce.BlazorApplication.Components.Components;

public partial class Categories(IProductCategoryHttpService productCategoryService) : ComponentBase
{
    private List<CategoryResponse>? ProductCategories { get; set; }

    protected override async Task OnInitializedAsync()
    {
        ProductCategories = await productCategoryService.GetClientCategories();
    }
}