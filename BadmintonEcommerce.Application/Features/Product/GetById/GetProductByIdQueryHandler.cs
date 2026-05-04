using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.Product.GetById;

public sealed class GetProductByIdQueryHandler(
    IProductRepository productRepository,
    IVariantCombinationRepository variantCombinationRepository,
    IProductOptionRepository productOptionRepository,
    IMapper mapper
    ) : IQueryHandler<GetProductByIdQuery, ProductDetailResponse>
{
    public async Task<Result<ProductDetailResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Catalog.Product> productQuery = await productRepository
            .Get(
                filter: filter => filter.Id == query.ProductId,
                orderBy: null,
                "Images,Category,Options,Variants");
        Domain.Entities.Catalog.Product? product = productQuery.FirstOrDefault();
        
        if (product == null)
            return Result.Failure<ProductDetailResponse>(
                ProductError.NotFound(query.ProductId));

        ProductDetailResponse response = new ProductDetailResponse()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Brand = product.Brand,
            Status = product.Status,
            CategoryId = product.CategoryId,
            Options = new List<ProductOptionResponse>(),
            Variants = new List<ProductVariantResponse>(),
            Images = new List<ProductDetailImageResponse>()
        };
        foreach (var item in product.Images)
        {
            response.Images.Add(new ProductDetailImageResponse()
            {
                Id = item.Id,
                ImageUrl = item.Url,
                IsPrimary = item.IsPrimary
            });
        }
        if (product.Variants.Count > 0)
            foreach (var item in product.Variants)
            {
                var variantOptions = await variantCombinationRepository.Get(
                    filter: filter => filter.VariantId == item.Id);
                List<Guid> optionValues = variantOptions.Select(v => v.OptionValueId).ToList();
                response.Variants.Add(new ProductVariantResponse()
                {
                    Id = item.Id,
                    OptionValues = optionValues,
                    Price = item.Price,
                    SKU = item.SKU
                });
            }
        if (product.Options.Count > 0)
            foreach (var item in product.Options)
            {
                var optionQuery = await productOptionRepository.Get(
                    filter: filter => filter.Id == item.Id,
                    null,
                    "OptionValues");
                ProductOption? option = optionQuery.FirstOrDefault();
                if (option != null)
                {
                    List<ProductOptionValueResponse> optionValues = new List<ProductOptionValueResponse>();
                    if (option.OptionValues.Count > 0)
                    {
                        foreach (var value in option.OptionValues)
                            optionValues.Add(new ProductOptionValueResponse()
                            {
                                Id = value.Id,
                                Value = value.Value
                            });
                    }

                    response.Options.Add(new ProductOptionResponse()
                    {
                        Id = item.Id,
                        Name = item.OptionName,
                        Values = optionValues
                    });
                }
            }
        return response;
    }
}