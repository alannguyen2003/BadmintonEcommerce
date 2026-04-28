using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;
using SharedKernel.Services;
using SharedKernel.Utils;

namespace BadmintonEcommerce.Application.Features.Product.Update;

public class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IProductCategoryRepository productCategoryRepository,
    IMapper mapper,
    IDateTimeProvider dateTimeProvider
    ) : ICommandHandler<UpdateProductCommand>
{
    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        //check if product exists
        Domain.Entities.Catalog.Product product = productRepository.GetById(command.Id);
        if (product == null)
            return Result.Failure(ProductError.NotFound(command.Id));
        //check if category exists
        Domain.Entities.Catalog.ProductCategory category = productCategoryRepository.GetById(command.CategoryId);

        if (category == null)
            return Result.Failure(ProductCategoryError.NotFound(command.CategoryId));

        product.Brand = command.Brand;
        product.Name = command.ProductName;
        product.Description = command.ProductDescription;
        product.Slug = SlugGenerateProvider.GenerateSlug(command.ProductName);
        product.CategoryId = command.CategoryId;
        product.LastModifiedOnUtc = dateTimeProvider.UtcNow;
        
        await productRepository.Update(product);
        await productRepository.SaveChangesAsync();
        
        return Result.Success();
    }
}