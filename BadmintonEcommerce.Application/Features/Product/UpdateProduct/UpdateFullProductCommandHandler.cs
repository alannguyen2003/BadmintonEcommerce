using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Abstraction.Services;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;
using SharedKernel.Utils;

namespace BadmintonEcommerce.Application.Features.Product.UpdateProduct;

public class UpdateFullProductCommandHandler(
    IProductRepository productRepository,
    IProductOptionRepository productOptionRepository,
    IProductOptionValueRepository productOptionValueRepository,
    IFileService fileService,
    IProductImageRepository productImageRepository,
    IMapper mapper
    ) : ICommandHandler<UpdateFullProductCommand>
{
    public async Task<Result> Handle(UpdateFullProductCommand command, CancellationToken cancellationToken)
    {
        //Check if product exists
        Domain.Entities.Catalog.Product product = productRepository.GetById(command.Id);
        if (product == null)
            return Result.Failure(ProductError.NotFound(command.Id));

        product.Brand = command.Brand;
        product.CategoryId = command.CategoryId;
        product.Description = command.Description;
        product.Name = command.Name;
        product.Slug = SlugGenerateProvider.GenerateSlug(command.Name);
        product.Status = command.Status;
        
        //Delete if any deleted images
        if (command.DeletedImages.Count > 0)
        {
            foreach (var item in command.DeletedImages)
                await productImageRepository.Delete(item);
        }
        
        //Delete if any deleted options
        if (command.DeletedOptions.Count > 0)
        {
            foreach (var item in command.DeletedOptions)
                await productOptionRepository.Delete(item);
        }
        
        //Delete if any deleted option values
        if (command.DeletedOptionValues.Count > 0)
        {
            foreach (var item in command.DeletedOptionValues) 
                await productOptionValueRepository.Delete(item);
        }

        await productRepository.SaveChangesAsync();
        return Result.Success();
    }
}