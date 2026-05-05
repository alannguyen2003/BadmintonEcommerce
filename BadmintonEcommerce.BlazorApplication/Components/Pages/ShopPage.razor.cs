using BadmintonEcommerce.BlazorApplication.Abstraction.Services;
using BadmintonEcommerce.Contracts.API.Presentation.Client.Category;
using Microsoft.AspNetCore.Components;

namespace BadmintonEcommerce.BlazorApplication.Components.Pages;

public partial class ShopPage(IProductCategoryHttpService productCategoryHttpService) : ComponentBase
{
    private List<CategoryResponse> Categories { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Categories = await productCategoryHttpService.GetClientCategories();
        Console.WriteLine(Categories.Count);
    }
}