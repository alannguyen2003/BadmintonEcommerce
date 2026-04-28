using BadmintonEcommerce.Application.Features.Product.Create;
using BadmintonEcommerce.Application.Features.ProductCategory.Create;
using BadmintonEcommerce.Application.Features.ProductCategory.Update;
using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Requests;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Mapper.Configurations;

namespace BadmintonEcommerce.Infrastructure.Persistence.Profiles.Catalog.Requests;

public class ProductCategoryProfile : IMappingProfile
{
    public void Configure(MapperConfiguration configuration)
    {
        configuration.CreateMap<CreateProductCategoryRequest, CreateProductCategoryCommand>()
            .ForMember(des => des.CategoryName,
                src => src.CategoryName)
            .ForMember(des => des.ParentCategoryId,
                src => src.ParantCategoryId.ToString() == string.Empty ? null : src.ParantCategoryId);
        
        configuration.CreateMap<CreateProductCategoryCommand, ProductCategory>()
            .ForMember(des => des.ParentCategoryId,
                src => src.ParentCategoryId.ToString() == string.Empty ? null : src.ParentCategoryId)
            .ForMember(des => des.CategoryName,
                src => src.CategoryName);

        configuration.CreateMap<UpdateProductCategoryRequest, UpdateProductCategoryCommand>()
            .ForMember(des => des.CategoryName,
                src => src.CategoryName)
            .ForMember(des => des.ParentCategoryId,
                src => src.ParentCategoryId.ToString() == string.Empty ? null : src.ParentCategoryId)
            .ForMember(des => des.Id,
                src => src.Id);
        
        configuration.CreateMap<UpdateProductCategoryCommand, ProductCategory>()
            .ForMember(des => des.Id,
                src => src.Id)
            .ForMember(des => des.CategoryName,
                src => src.CategoryName)
            .ForMember(des => des.ParentCategoryId,
                src => src.ParentCategoryId.ToString() == string.Empty ? null : src.ParentCategoryId);
    }
}