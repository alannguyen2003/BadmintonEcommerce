using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;

namespace BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Responses;

public class ProductCategoryByIdResponse
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; }
    public string ParentCategoryName { get; set; }
    public List<ProductResponse> Products { get; set; }
}