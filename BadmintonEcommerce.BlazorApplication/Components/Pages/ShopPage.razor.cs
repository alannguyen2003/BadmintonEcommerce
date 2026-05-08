using BadmintonEcommerce.BlazorApplication.Abstraction.Services;
using BadmintonEcommerce.BlazorApplication.Utils;
using BadmintonEcommerce.Contracts.API.Presentation;
using BadmintonEcommerce.Contracts.API.Presentation.Client.Category;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using Microsoft.AspNetCore.Components;

namespace BadmintonEcommerce.BlazorApplication.Components.Pages;

public partial class ShopPage(IProductCategoryHttpService productCategoryHttpService,
    IProductHttpService productHttpService) : ComponentBase
{
    private List<CategoryResponse> Categories { get; set; }
    
    [Parameter]
    public Guid? CategoryId { get; set; }
    
    public PagedRequest<Guid> PagedRequest { get; set; } = new()
    {
        PageNumber = 1,
        PageSize = 9,
        Data = Guid.NewGuid()
    };

    private int FirstElementOfPage => Products != null ? (Products.PageNumber - 1) * Products.PageSize + 1 : 0;
    private int LastElementOfPage => Products != null ? 
        (Products.PageNumber * Products.PageSize) > Products.TotalCount ? Products.TotalCount : (Products.PageNumber * Products.PageSize)
        : 0;

    public PagedList<List<ProductResponse>> Products { get; set; } = new PagedList<List<ProductResponse>>();

    protected override async Task OnInitializedAsync()
    {
        Categories = await productCategoryHttpService.GetClientCategories();
        int currentPage = 0;
        navigationManager.TryGetQueryString<int>("page", out currentPage);
        PagedRequest.PageNumber = currentPage;
        if (CategoryId != null) PagedRequest.Data = CategoryId.Value;
        Products = await productHttpService.GetProductsByCategoryAndDefault(PagedRequest);
        if (currentPage == 0) Products.PageNumber = 1;
    }

    private string GetAbsoluteUri()
    {
        var uri = new Uri(navigationManager.Uri);

        return uri.AbsolutePath;
    }

    private string Paging(int page)
    {
        return GetAbsoluteUri() + "?page=" + page;
    }
}