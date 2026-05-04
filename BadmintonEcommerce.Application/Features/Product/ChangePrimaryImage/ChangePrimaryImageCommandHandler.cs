using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Errors;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.Product.ChangePrimaryImage;

public class ChangePrimaryImageCommandHandler(
    IProductRepository productRepository,
    IProductImageRepository productImageRepository)
        : ICommandHandler<ChangePrimaryImageCommand>
{
    public async Task<Result> Handle(ChangePrimaryImageCommand command, CancellationToken cancellationToken)
    {
        //Check if product image exists
        ProductImage image = productImageRepository.GetById(command.ImageId);
        if (image == null)
            return Result.Failure(ProductImageError.NotFound(command.ImageId));

        IEnumerable<ProductImage> images = await productImageRepository.Get(
            filter: filter => filter.ProductId == command.ProductId);

        foreach (var item in images)
        {
            if (item.Id != command.ImageId) 
                item.IsPrimary = false;
            else item.IsPrimary = true;
        }

        await productImageRepository.SaveChangesAsync();
        return Result.Success();
    }
}