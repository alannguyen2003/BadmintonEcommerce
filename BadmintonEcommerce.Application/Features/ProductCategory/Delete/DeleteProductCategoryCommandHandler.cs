using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Errors;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.ProductCategory.Delete;

public class DeleteProductCategoryCommandHandler(
    IProductCategoryRepository productCategoryRepository)
    : ICommandHandler<DeleteProductCategoryCommand>
{
    public async Task<Result> Handle(DeleteProductCategoryCommand command, CancellationToken cancellationToken)
    {
        Domain.Entities.Catalog.ProductCategory? category = productCategoryRepository.GetById(command.ProductCategoryId);

        if (category is null)
        {
            return Result.Failure(ProductCategoryError.NotFound(command.ProductCategoryId));
        }
        
        await productCategoryRepository.Delete(category);
        await productCategoryRepository.SaveChangesAsync();
        return Result.Success();
    }
}