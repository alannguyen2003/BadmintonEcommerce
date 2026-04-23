namespace BadmintonEcommerce.Application.Features.ProductCategory.Get;

public class ProductCategoryResponse
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; }
    public string? ParentCategoryName { get; set; }
}