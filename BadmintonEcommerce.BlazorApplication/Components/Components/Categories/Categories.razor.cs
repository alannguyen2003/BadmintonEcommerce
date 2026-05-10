using BadmintonEcommerce.BlazorApplication.Abstraction.Services;
using BadmintonEcommerce.Contracts.API.Presentation.Client.Category;
using Microsoft.AspNetCore.Components;

namespace BadmintonEcommerce.BlazorApplication.Components.Components.Categories;

public partial class Categories(IProductCategoryHttpService productCategoryService) : ComponentBase
{
    private List<CategoryResponse>? ProductCategories { get; set; }

    protected override async Task OnInitializedAsync()
    {
        ProductCategories = await productCategoryService.GetClientCategories();
        Console.WriteLine($"{ProductCategories.Count} categories have been retrieved.");
    }
}