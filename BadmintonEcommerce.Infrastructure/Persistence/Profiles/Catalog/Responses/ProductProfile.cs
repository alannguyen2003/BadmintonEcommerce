using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Mapper.Configurations;

namespace BadmintonEcommerce.Infrastructure.Persistence.Profiles.Catalog.Responses;

public class ProductProfile : IMappingProfile
{
    public void Configure(MapperConfiguration configuration)
    {
        configuration.CreateMap<Product, ProductResponse>()
            .ForMember(des => des.ProductName,
                src => src.Name)
            .ForMember(des => des.ProductDescription,
                src => src.Description)
            .ForMember(des => des.CategoryName,
                src => (src.Category.CategoryName) ?? string.Empty)
            .ForMember(des => des.Brand,
                src => src.Brand)
            .ForMember(des => des.Slug,
                src => src.Slug)
            .ForMember(des => des.CategoryId,
                src => src.CategoryId)
            .ForMember(des => des.TotalVariants,
                src => src.Variants.Count)
            .ForMember(des => des.Price,
                src => src.Variants.Min(item => item.Price))
            .ForMember(des => des.PrimaryImage,
                src => src.Images.Count == 0 ? null : new PrimaryImageResponse()
                {
                    Id = src.Images.First(item => item.IsPrimary).Id,
                    Url = src.Images.First(item => item.IsPrimary).Url
                });


        configuration.CreateMap<Product, ProductDetailResponse>()
            .ForMember(des => des.Id,
                src => src.Id)
            .ForMember(des => des.Name,
                src => src.Name)
            .ForMember(des => des.Description,
                src => src.Description)
            .ForMember(des => des.Price,
                src => src.Variants.Min(item => item.Price))
            .ForMember(des => des.CategoryId,
                src => src.CategoryId)
            .ForMember(des => des.Brand,
                src => src.Brand)
            .ForMember(des => des.Slug,
                src => src.Slug)
            .ForMember(des => des.CategoryName,
                src => src.Category == null ? string.Empty : src.Category.CategoryName)
            .ForMember(des => des.Status,
                src => src.Status)
            .ForMember(des => des.Images,
                src => src.Images.Count == 0 ? new List<ProductDetailImageResponse>() : 
                    src.Images.Select(item => new ProductDetailImageResponse()
                        {
                            Id = item.Id,
                            ImageUrl = item.Url
                        }).ToList())
            .ForMember(des => des.Options,
                src => src.Options.Count == 0 ? new List<ProductOptionResponse>() :
                    src.Options.Select(item => new ProductOptionResponse()
                        {
                            Id = item.Id,
                            Name = item.OptionName
                        }).ToList())
            .ForMember(des => des.Variants,
                src => new List<ProductVariantResponse>())
            .ReverseMap();
    }
}