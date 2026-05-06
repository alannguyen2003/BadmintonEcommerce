using BadmintonEcommerce.Contracts.API.Presentation.Client.Category;
using Microsoft.AspNetCore.Components;

namespace BadmintonEcommerce.BlazorApplication.Components.Components.Categories;

public partial class CategoryItem : ComponentBase
{
    [Parameter]
    public CategoryResponse Category { get; set; }
}