using BadmintonEcommerce.BlazorApplication.Abstraction.Services;
using BadmintonEcommerce.Contracts.API.Presentation;
using BadmintonEcommerce.Contracts.API.Presentation.Client.Category;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using Microsoft.AspNetCore.Components;

namespace BadmintonEcommerce.BlazorApplication.Components.Pages;

public partial class ShopPage(IProductCategoryHttpService productCategoryHttpService,
    IProductHttpService productHttpService) : ComponentBase
{
    private List<CategoryResponse> Categories { get; set; }

    public PagedRequest<Guid> PagedRequest { get; set; } = new()
    {
        PageNumber = 1,
        PageSize = 20,
        Data = Guid.NewGuid()
    };

    public PagedList<List<ProductResponse>> Products { get; set; } = new PagedList<List<ProductResponse>>();

    protected override async Task OnInitializedAsync()
    {
        Categories = await productCategoryHttpService.GetClientCategories();
        Products = await productHttpService.GetProductsByCategoryAndDefault(PagedRequest);
        Console.WriteLine(Products.Data.Count);
    }
}