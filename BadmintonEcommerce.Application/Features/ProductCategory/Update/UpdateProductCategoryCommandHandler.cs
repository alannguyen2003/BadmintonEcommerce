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
        
        category.CategoryName = command.CategoryName;
        category.ParentCategoryId = command.ParentCategoryId;
        category.LastModifiedOnUtc = dateTimeProvider.UtcNow;
        
        await productCategoryRepository.Update(category);
        await productCategoryRepository.SaveChangesAsync();
        return Result.Success();
    }
}