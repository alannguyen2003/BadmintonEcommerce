using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Errors;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.Product.Delete;

public class DeleteProductCommandHandler(
    IProductRepository productRepository) 
    : ICommandHandler<DeleteProductCommand>
{
    public async Task<Result> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        //check if product exists
        Domain.Entities.Catalog.Product product = productRepository.GetById(command.ProductId);

        if (product == null)
            return Result.Failure(ProductError.NotFound(command.ProductId));

        await productRepository.Delete(product);
        await productRepository.SaveChangesAsync();

        return Result.Success();
    }
}