using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Errors;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Application.Features.ProductCategory.Create;

public class CreateProductCategoryCommandHandler(
    IProductCategoryRepository productCategoryRepository,
    IDateTimeProvider dateTimeProvider) 
    : ICommandHandler<CreateProductCategoryCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProductCategoryCommand command, CancellationToken cancellationToken)
    {
        int level = 1;
        if (command.ParentCategoryId != null)
        {
            Domain.Entities.Catalog.ProductCategory category =
                productCategoryRepository.GetById(command.ParentCategoryId);
            if (category == null)
                return Result.Failure<Guid>(ProductCategoryError.NotFound((Guid)command.ParentCategoryId));
            level = category.Level + 1;
        }
        var productCategory = new Domain.Entities.Catalog.ProductCategory()
        {
            CategoryName = command.CategoryName,
            ParentCategoryId = command.ParentCategoryId != null ? command.ParentCategoryId : null,
            CreatedOnUtc = dateTimeProvider.UtcNow,
            Level = level
        };
        productCategoryRepository.Insert(productCategory);
        await productCategoryRepository.SaveChangesAsync();
        return productCategory.Id;
    }

    public bool CheckIfTheLevelIsMoreThan3(int currentLevel)
    {
        if (currentLevel == 3) return false;
        return true;
    }
}