using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.Client.GetProductDetail;

public class GetProductDetailQueryHandler(
    IProductRepository productRepository,
    IProductImageRepository productImageRepository,
    IProductOptionRepository productOptionRepository, 
    IProductVariantRepository productVariantRepository,
    IMapper mapper) : IQueryHandler<GetProductDetailQuery, ProductDetailResponse>
{
    public async Task<Result<ProductDetailResponse>> Handle(GetProductDetailQuery query, CancellationToken cancellationToken)
    {
        //Check if the product exists
        Domain.Entities.Catalog.Product? productCheck = productRepository.GetById(query.ProductId);
        if (productCheck is null)
            return Result.Failure<ProductDetailResponse>(ProductError.NotFound(query.ProductId));

        IEnumerable<Domain.Entities.Catalog.Product> productsQuery = await productRepository.Get(
            filter: filter => filter.Id.Equals(query.ProductId),
            orderBy: null,
            includeProperties: "Images,Category,Options,Variants");
        
        Domain.Entities.Catalog.Product product = productsQuery.ToList().First();
        ProductDetailResponse response = mapper.Map<ProductDetailResponse>(product);
        foreach (var item in response.Options)
        {
            var option = await productOptionRepository.Get(
                filter: filter => filter.Id.Equals(item.Id),
                orderBy: null,
                includeProperties: "OptionValues");
            item.Values = new List<ProductOptionValueResponse>();
            item.Values.AddRange(option.ToList()
                .First().OptionValues.Select(option => new ProductOptionValueResponse()
                    {
                        Id = option.Id,
                        Value = option.Value
                    }).ToList());
        }

        foreach (var item in product.Variants)
        {
            var variantResponse = new ProductVariantResponse();
            variantResponse.Id = item.Id;
            var variant = await productVariantRepository.Get(
                filter: filter => filter.Id.Equals(item.Id),
                orderBy: null,
                includeProperties: "Combinations,InventoryItem");
            variantResponse.OptionValues = new List<Guid>();
            variantResponse.Price = variant.ToList().First().Price;
            variantResponse.SKU = variant.ToList().First().SKU;
            variantResponse.IsAvailable = variant.ToList().First().InventoryItem.Quantity > 0;
            variantResponse.OptionValues.AddRange(variant.ToList()
                .First().Combinations.Select(v => v.OptionValueId).ToList());
            response.Variants.Add(variantResponse);
        }
        return response;
    }
}