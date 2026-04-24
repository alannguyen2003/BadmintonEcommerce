using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.ProductCategory.Create;

public class CreateProductCategoryCommandHandler(
    IProductCategoryRepository productCategoryRepository) 
    : ICommandHandler<CreateProductCategoryCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProductCategoryCommand command, CancellationToken cancellationToken)
    {
        var productCategory = new Domain.Entities.Catalog.ProductCategory()
        {
            CategoryName = command.CategoryName,
            ParentCategoryId = command.ParentCategoryId != null ? command.ParentCategoryId : null,
        };
        productCategoryRepository.Insert(productCategory);
        await productCategoryRepository.SaveChangesAsync();
        return productCategory.Id;
    }
}