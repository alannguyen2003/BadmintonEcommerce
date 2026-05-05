namespace BadmintonEcommerce.Contracts.API.Presentation.Client.Category;

public class CategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<CategoryResponse> ChildCategories { get; set; }
}