using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Abstraction.Services;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Errors;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Application.Features.Product.UpdateProductImages;

public class UpdateProductImageCommandHandler(
    IProductRepository productRepository,
    IProductImageRepository productImageRepository,
    IFileService fileService,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<UpdateProductImageCommand>
{
    public async Task<Result> Handle(UpdateProductImageCommand command, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Catalog.Product> productsQuery = await productRepository.Get(
            filter: filter => filter.Id.Equals(command.ProductId),
            orderBy: null,
            includeProperties: "Images");
        if (!productsQuery.Any())
            return Result.Failure(ProductError.NotFound(command.ProductId));

        foreach (var item in command.DeletedImages)
        {
            var imageFind = productsQuery.First()
                .Images.Where(image => image.Id.Equals(item)).FirstOrDefault();
            Console.WriteLine(imageFind == null);
            productsQuery.First().Images.Remove(imageFind);
        }
        
        foreach (var item in command.AddedImages)
        {
            var fileUploaded = await fileService.UploadFileAsync(
                new FileUploadStream()
                {
                    FileName = item.FileName,
                    ContentType = item.ContentType,
                    Stream = item.Stream
                });
            productsQuery.First().Images.Add(new ProductImage()
            {
                IsPrimary = false,
                ProductId = productsQuery.First().Id,
                Url = fileUploaded.SecureUrl.AbsoluteUri,
                ImageMetadata = fileUploaded.DisplayName,
                CreatedOnUtc = dateTimeProvider.UtcNow,
            });
        }

        ProductImage? primaryImage = productsQuery
            .First()
            .Images.Where(item => item.IsPrimary == true)
            .FirstOrDefault();
        if (primaryImage == null) 
            if (productsQuery.First().Images.Any()) productsQuery.First().Images.First().IsPrimary = true;
        productsQuery.First().LastModifiedOnUtc = dateTimeProvider.UtcNow;
        await productRepository.SaveChangesAsync();
        return Result.Success();
    }
}