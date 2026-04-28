using BadmintonEcommerce.Application.Features.ProductCategory.Get;
using BadmintonEcommerce.Application.Features.ProductCategory.GetById;
using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Responses;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Mapper.Configurations;

namespace BadmintonEcommerce.Infrastructure.Persistence.Profiles.Catalog.Responses;

public class ProductCategoryProfile : IMappingProfile
{
    public void Configure(MapperConfiguration configuration)
    {
        configuration.CreateMap<ProductCategory, ProductCategoryResponse>()
            .ForMember(des => des.CategoryName, 
                src => src.CategoryName)
            .ForMember(des => des.ParentCategoryName, 
                src => (src.ParentCategory != null ? src.ParentCategory.CategoryName : null) ?? string.Empty)
            .ReverseMap();
        
        configuration.CreateMap<ProductCategory, ProductCategoryByIdResponse>() 
            .ForMember(des => des.CategoryName, 
                src => src.CategoryName)    
            .ForMember(des => des.ParentCategoryName, 
                src => (src.ParentCategory != null ? src.ParentCategory.CategoryName : null) ?? string.Empty)
            .ReverseMap();
    }
}