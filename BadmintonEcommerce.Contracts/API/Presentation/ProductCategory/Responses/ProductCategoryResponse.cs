namespace BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Responses;

public class ProductCategoryResponse
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; }
    public int Level { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
}