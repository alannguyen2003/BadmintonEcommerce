namespace BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Requests;

public sealed class UpdateProductCategoryRequest
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; }
    public Guid? ParentCategoryId { get; set; }
}