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
                src => (src.Category != null ? src.Category.CategoryName : null) ?? string.Empty)
            .ForMember(des => des.Brand,
                src => src.Brand)
            .ForMember(des => des.Slug,
                src => src.Slug);
    }
}