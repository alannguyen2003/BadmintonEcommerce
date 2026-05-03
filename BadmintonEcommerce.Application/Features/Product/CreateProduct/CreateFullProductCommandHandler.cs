using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Abstraction.Services;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Entities.Inventory;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;
using SharedKernel.Services;
using SharedKernel.Utils;

namespace BadmintonEcommerce.Application.Features.Product.CreateProduct;

public class CreateFullProductCommandHandler(
    IProductCategoryRepository productCategoryRepository,
    IProductRepository productRepository,
    IProductVariantRepository productVariantRepository,
    IProductImageRepository productImageRepository,
    IProductOptionRepository productOptionRepository,
    IMapper mapper,
    IInventoryItemRepository inventoryItemRepository,
    IDateTimeProvider dateTimeProvider,
    IFileService fileService
    ) : ICommandHandler<CreateFullProductCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateFullProductCommand command, CancellationToken cancellationToken)
    {
        //Check if category exists
        Domain.Entities.Catalog.ProductCategory category = productCategoryRepository.GetById(command.ProductCategoryId);
        if (category == null)
            return Result.Failure<Guid>(ProductCategoryError.NotFound(command.ProductCategoryId));
        
        //Check if any duplicate options
        if (CheckIfAnyOptionDuplicate(command.OptionRequests))
            return Result.Failure<Guid>(ProductError.NotFound(new Guid()));
        
        //Add Product
        Domain.Entities.Catalog.Product product = new Domain.Entities.Catalog.Product()
        {
            Name = command.ProductName,
            Brand = command.Brand,
            Description = command.ProductDescription,
            CategoryId = command.ProductCategoryId,
            Slug = SlugGenerateProvider.GenerateSlug(command.ProductName),
            CreatedOnUtc = dateTimeProvider.UtcNow,
            Images = new List<Domain.Entities.Catalog.ProductImage>(),
            Options =  new List<Domain.Entities.Catalog.ProductOption>(),
            Variants = new List<Domain.Entities.Catalog.ProductVariant>()
        };

        foreach (var item in command.Files)
        {
            var result = await fileService.UploadFileAsync(new FileUploadStream()
            {
                FileName = item.FileName,
                ContentType = item.ContentType,
                Stream = item.Stream,
            });
            product.Images.Add(new Domain.Entities.Catalog.ProductImage()
            {
                IsPrimary = false,
                ImageMetadata = result.DisplayName,
                Url = result.SecureUrl.AbsoluteUri,
                CreatedOnUtc = dateTimeProvider.UtcNow,
            });
        }

        var optionValueMap = new Dictionary<(string code, string value), ProductOptionValue>();
        foreach (var item in command.OptionRequests)
        {
            ProductOption productOption = new ProductOption()
            {
                OptionName = item.Name,
                Code = item.Code,
                OptionValues = new List<ProductOptionValue>()
            };
            foreach (var value in item.Values)
            {
                ProductOptionValue optionValue = new ProductOptionValue()
                {
                    Id = Guid.NewGuid(),
                    Value = value,
                    CreatedOnUtc = dateTimeProvider.UtcNow,
                    Option = productOption
                };
                productOption.OptionValues.Add(optionValue);
                
                //Khúc này tạo map
                optionValueMap[(item.Code.ToLower(), value.ToLower())] = optionValue;
            }
            product.Options.Add(productOption);
        }

        foreach (var item in command.VariantRequests)
        {
            ProductVariant productVariant = new ProductVariant()
            {
                Id = Guid.NewGuid(),
                CreatedOnUtc = dateTimeProvider.UtcNow,
                Price = item.Price,
                SKU = new Guid().ToString(),
                Combinations = new List<VariantCombination>()
            };
            InventoryItem inventoryItem = new InventoryItem()
            {
                Id  = Guid.NewGuid(),
                Quantity = item.Stock,
                CreatedOnUtc = dateTimeProvider.UtcNow,
            };
            inventoryItem.Variant = productVariant;
            
            //Tạo combinations
            foreach (var combination in item.Values)
            {
                var key = (combination.Code.ToLower(), combination.Value.ToLower());

                if (!optionValueMap.TryGetValue(key, out var optionValue))
                {
                    throw new Exception($"Option value not found: {combination.Code} - {combination.Value}");
                }
                
                productVariant.Combinations.Add(new VariantCombination()
                {
                    Variant = productVariant,
                    OptionValue = optionValue
                });
            }
            
            product.Variants.Add(productVariant);
        }
        productRepository.Insert(product);
        await productRepository.SaveChangesAsync();
        return Result.Success(product.Id);
    }

    private bool CheckIfAnyOptionDuplicate(List<CreateOptionRequest> options)
    {
        return options.GroupBy(item => item.Code)
            .Any(item => item.Count() > 1);
    }
}