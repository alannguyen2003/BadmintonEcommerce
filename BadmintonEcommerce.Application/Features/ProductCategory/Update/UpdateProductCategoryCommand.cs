using BadmintonEcommerce.Application.Abstraction.Messaging;

namespace BadmintonEcommerce.Application.Features.ProductCategory.Update;

public class UpdateProductCategoryCommand : ICommand
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; }
    public Guid? ParentCategoryId { get; set; }
}