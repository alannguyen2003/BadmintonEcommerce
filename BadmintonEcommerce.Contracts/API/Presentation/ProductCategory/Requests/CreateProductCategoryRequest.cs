namespace BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Requests;

public sealed class CreateProductCategoryRequest
{
    public string CategoryName { get; set; }
    public Guid? ParantCategoryId { get; set; }
}