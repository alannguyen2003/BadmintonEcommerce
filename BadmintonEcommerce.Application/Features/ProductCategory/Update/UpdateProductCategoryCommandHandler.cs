using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Errors;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Application.Features.ProductCategory.Update;

public class UpdateProductCategoryCommandHandler(
    IProductCategoryRepository productCategoryRepository,
    IMapper mapper,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdateProductCategoryCommand>
{
    public async Task<Result> Handle(UpdateProductCategoryCommand command, CancellationToken cancellationToken)
    {
        Domain.Entities.Catalog.ProductCategory category = productCategoryRepository.GetById(command.Id);
        if (category is null)
        {
            return Result.Failure(ProductCategoryError.NotFound(command.Id));
        }

        int level = 1;
        Domain.Entities.Catalog.ProductCategory parentCategory =
            productCategoryRepository.GetById(command.ParentCategoryId);
        if (parentCategory is not null)
        {
            if (CheckIfTheParentCategoryLevelIsMoreThan3(parentCategory.Level))
                return Result.Failure(ProductCategoryError.CannotCreateMoreThan3Levels());
            level = parentCategory.Level + 1;
        }

        category.CategoryName = command.CategoryName;
        category.ParentCategoryId = command.ParentCategoryId;
        category.LastModifiedOnUtc = dateTimeProvider.UtcNow;
        category.Level = level;
        
        await productCategoryRepository.Update(category);
        await productCategoryRepository.SaveChangesAsync();
        return Result.Success();
    }

    private bool CheckIfTheParentCategoryLevelIsMoreThan3(int currentLevel)
    {
        if (currentLevel == 3) return true;
        return false;
    }
}