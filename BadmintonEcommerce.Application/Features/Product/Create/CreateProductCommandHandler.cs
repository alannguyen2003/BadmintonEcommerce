using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Application.Features.Product.Create;

public class CreateProductCommandHandler(
    IProductRepository productRepository,
    IProductCategoryRepository productCategoryRepository,
    IMapper mapper,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<CreateProductCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        Domain.Entities.Catalog.ProductCategory? category = productCategoryRepository.GetById(command.CategoryId);

        if (category == null)
            return Result.Failure<Guid>(ProductCategoryError.NotFound(command.CategoryId));

        Domain.Entities.Catalog.Product product = mapper.Map<Domain.Entities.Catalog.Product>(command);
        product.CreatedOnUtc = dateTimeProvider.UtcNow;
        
        productRepository.Insert(product);
        await productRepository.SaveChangesAsync();
        
        return Result.Success(product.Id);
    }
}