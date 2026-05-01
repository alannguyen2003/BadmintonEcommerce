using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Abstraction.Services;
using BadmintonEcommerce.Contracts.API.Presentation.File.Request;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Errors;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Application.Features.Product.UploadProductImage;

public class UploadProductImageCommandHandler(
    IProductImageRepository productImageRepository,
    IFileService fileService,
    IProductRepository productRepository,
    IDateTimeProvider dateTimeProvider
    ) : ICommandHandler<UploadProductImageCommand, List<ProductImageResponse>>
{
    public async Task<Result<List<ProductImageResponse>>> Handle(UploadProductImageCommand command, CancellationToken cancellationToken)
    {
        Domain.Entities.Catalog.Product product = productRepository.GetById(command.ProductId);
        //Check if the product exists
        if (product is null)
            return Result.Failure<List<ProductImageResponse>>(ProductError.NotFound(command.ProductId));
        List<ProductImageResponse> responses = new List<ProductImageResponse>(); 
        foreach (FileUploadStreamData item in command.Files)
        {
            var result = await fileService.UploadFileAsync(new FileUploadStream()
            {
                FileName = item.FileName,
                ContentType = item.ContentType,
                Stream = item.Stream
            });
            responses.Add(new ProductImageResponse()
            {
                ImageUrl = result.SecureUrl.AbsoluteUri,
                Metadata = result.DisplayName,
                IsMainProfile = false,
                ProductName = product.Name
            });
            productImageRepository.Insert(new ProductImage()
            {
                ProductId = command.ProductId,
                Url = result.SecureUrl.AbsoluteUri,
                ImageMetadata = result.DisplayName,
                CreatedOnUtc = dateTimeProvider.UtcNow,
                IsPrimary = item.FileName.Equals(command.Files[0].FileName) ? true : false
            });
        }
        await productImageRepository.SaveChangesAsync();
        return responses;
    }
}