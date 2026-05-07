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
            .ForMember(des => des.PrimaryImage,
                src => src.Images.Count == 0 ? null : new PrimaryImageResponse()
                {
                    Id = src.Images.First(item => item.IsPrimary).Id,
                    Url = src.Images.First(item => item.IsPrimary).Url
                });
        
    }
}