using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Entities.Inventory;
using BadmintonEcommerce.Domain.Enums;
using BadmintonEcommerce.Domain.Errors;
using SharedKernel.Patterns;
using SharedKernel.Services;
using SharedKernel.Utils;

namespace BadmintonEcommerce.Application.Features.Product.UpdateOption;

public class UpdateOptionCommandHandler(
    IProductRepository productRepository,
    IProductOptionRepository productOptionRepository,
    IProductOptionValueRepository productOptionValueRepository,
    IProductVariantRepository productVariantRepository,
    IInventoryItemRepository inventoryItemRepository,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<UpdateOptionCommand>
{
    public async Task<Result> Handle(UpdateOptionCommand command, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Catalog.Product> products = await productRepository.Get(
            filter: filter => filter.Id.Equals(command.ProductId),
            orderBy: null,
            includeProperties: "Options,Variants");
        Domain.Entities.Catalog.Product product = products.First();
        if (product == null)
        {
            return Result.Failure<Tuple<int, int>>(ProductError.NotFound(command.ProductId));
        }
        
        //Handled Delete
        foreach (var item in command.DeletedVariants)
            await productVariantRepository.Delete(item);
        foreach (var item in command.DeletedOptions) 
            await productOptionRepository.Delete(item);
         
        //Handle Added 
        /*Tuple<List<ProductOption>, List<ProductVariant>> tuples = await this.HandleAddedOptions(command.AddedOptions,
            command.AddedVariants,
            product.Slug);
        if (product.Options == null) product.Options = new List<ProductOption>();
        if (product.Variants == null) product.Variants = new List<ProductVariant>();
        foreach (var item in tuples.Item1)
        {
            productOptionRepository.Insert(new ProductOption()
            {
                ProductId = product.Id,
                DataType = OptionValueDataType.String,
                OptionName = item.OptionName,
                Code = item.Code,
                OptionValues = item.OptionValues
            });
        }

        foreach (var item in tuples.Item2)
        {
            ProductVariant variant = new ProductVariant()
            {
                ProductId = product.Id,
                Price = item.Price,
                SKU = item.SKU,
                Combinations = item.Combinations
            };
            InventoryItem inventory = new InventoryItem()
            {
                Quantity = 0,
                Reserved = 0,
                Variant = variant,
                CreatedOnUtc = dateTimeProvider.UtcNow
            };
            productVariantRepository.Insert(variant);
            inventoryItemRepository.Insert(inventory);
        }*/
        var optionValueMap = new Dictionary<(string code, string value), ProductOptionValue>();
        foreach (var item in command.AddedOptions)
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

        foreach (var item in command.AddedVariants)
        {
            ProductVariant productVariant = new ProductVariant()
            {
                Id = Guid.NewGuid(),
                CreatedOnUtc = dateTimeProvider.UtcNow,
                Price = item.Price,
                SKU = 
                    SlugGenerateProvider.GenerateSku(product.Slug, 
                        item.Values.Select(v => (v.Code, v.Value)).ToList()),
                Combinations = new List<VariantCombination>()
            };
            InventoryItem inventoryItem = new InventoryItem()
            {
                Id  = Guid.NewGuid(),
                Quantity = item.Stock,
                CreatedOnUtc = dateTimeProvider.UtcNow,
            };
            inventoryItem.Variant = productVariant;
            inventoryItemRepository.Insert(inventoryItem);
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
        

        await productRepository.Update(product);
        await productRepository.SaveChangesAsync();
        return Result.Success();
    }

    private async Task<Tuple<List<ProductOption>, List<ProductVariant>>> HandleAddedOptions(List<AddedOptionRequest> addedOptions,
        List<AddedVariantRequest> addedVariants,
        string productSlug)
    {
        List<ProductOption> productOptions = new List<ProductOption>();
        List<ProductVariant> productVariants = new List<ProductVariant>();
        var optionValueMap = new Dictionary<(string code, string value), ProductOptionValue>();
        foreach (var item in addedOptions)
        {
            ProductOption productOption = new ProductOption()
            {
                OptionName = item.Name,
                Code = item.Code,
                OptionValues = new List<ProductOptionValue>()
            };
            foreach (var value in item.AddedValues)
            {
                ProductOptionValue optionValue = new ProductOptionValue()
                {
                    Id = Guid.NewGuid(),
                    Value = value,
                    CreatedOnUtc = dateTimeProvider.UtcNow,
                    Option = productOption
                };
                productOption.OptionValues.Add(optionValue);

                optionValueMap[(item.Code.ToLower(), value.ToLower())] = optionValue;
            }
            productOptions.Add(productOption);
        }

        foreach (var item in addedVariants)
        {
            ProductVariant productVariant = new ProductVariant()
            {
                Id = Guid.NewGuid(),
                CreatedOnUtc = dateTimeProvider.UtcNow,
                Price = item.Price,
                SKU = SlugGenerateProvider.GenerateSku(
                    productSlug, item.Values.Select(item => (item.Code, item.Value)).ToList()),
                Combinations = new List<VariantCombination>()
            };
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
            productVariants.Add(productVariant);
        }
        return new Tuple<List<ProductOption>, List<ProductVariant>>(productOptions, productVariants);
    }
}