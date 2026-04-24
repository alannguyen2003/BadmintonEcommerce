using BadmintonEcommerce.Application.Features.ProductCategory.Get;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Mapper.Configurations;

namespace BadmintonEcommerce.Application.Abstraction.Profile;


public class ProductCategoryProfile : IMappingProfile
{
    public void Configure(MapperConfiguration configuration)
    {
        configuration.CreateMap<ProductCategory, ProductCategoryResponse>()
            .ForMember(des => des.CategoryName, src => src.CategoryName)
            .ForMember(des => des.ParentCategoryName, src => src.ParentCategory.ParentCategory)
            .ReverseMap();
        
        
    }
}