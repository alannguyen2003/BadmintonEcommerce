using BadmintonEcommerce.BlazorApplication.Abstraction.Services;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BadmintonEcommerce.BlazorApplication.Components.Components.Products;

public partial class ProductDetail(
    IProductHttpService productHttpService) : ComponentBase
{
    public ProductDetailResponse? Product { get; set; } = new ProductDetailResponse()
    {
        Options = new List<ProductOptionResponse>(),
        Images = new List<ProductDetailImageResponse>(),
        Variants = new List<ProductVariantResponse>()
    };
    
    private Dictionary<Guid, Guid>
        SelectedOptions = new();
    
    
    
    private ProductVariantResponse? CurrentVariant { get; set; }
    
    [Parameter]
    public Guid ProductId { get; set; }
    protected override async Task OnInitializedAsync()
    {
        Product = await productHttpService.GetClientProductDetailResponse(ProductId);
        Console.WriteLine(Product.Options.Count);
    }
    
    private bool IsSelected(
        Guid optionId,
        Guid valueId)
    {
        return SelectedOptions.TryGetValue(
                   optionId,
                   out var selectedValueId)
               &&
               selectedValueId == valueId;
    }
    
    private void SelectOption(Guid optionId, Guid valueId)
    {
        SelectedOptions[optionId] = valueId;
        Console.WriteLine("Triggered!");
        UpdateCurrentVariant();
    }
    
    private void UpdateCurrentVariant()
    {
        var selectedValueIds =
            SelectedOptions.Values.ToList();

        CurrentVariant =
            Product.Variants.FirstOrDefault(
                variant =>
                    variant.OptionValues.All(
                        x => selectedValueIds.Contains(x))
                    &&
                    variant.OptionValues.Count ==
                    selectedValueIds.Count);
    }
    
    
}