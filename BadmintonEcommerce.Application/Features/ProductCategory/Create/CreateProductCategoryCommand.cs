using BadmintonEcommerce.Application.Abstraction.Messaging;

namespace BadmintonEcommerce.Application.Features.ProductCategory.Create;

public sealed class CreateProductCategoryCommand : ICommand<Guid>
{
    public string CategoryName { get; set; }
    public int Level { get; set; }
    public Guid? ParentCategoryId { get; set; }
}