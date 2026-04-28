using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.Product.Create;

public class CreateProductCommandHandler(
    IProductRepository productRepository,
    IProductCategoryRepository productCategoryRepository,
    IMapper mapper) : ICommandHandler<CreateProductCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        Domain.Entities.Catalog.ProductCategory? category = productCategoryRepository.GetById(command.CategoryId);

        if (category == null)
            return Result.Failure<Guid>(ProductCategoryError.NotFound(command.CategoryId));

        Domain.Entities.Catalog.Product product = mapper.Map<Domain.Entities.Catalog.Product>(command);
        
        productRepository.Insert(product);
        await productRepository.SaveChangesAsync();
        
        return Result.Success(product.Id);
    }
}