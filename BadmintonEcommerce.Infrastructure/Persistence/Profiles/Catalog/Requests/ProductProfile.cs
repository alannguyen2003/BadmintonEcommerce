using BadmintonEcommerce.Application.Features.Product.Create;
using BadmintonEcommerce.Application.Features.Product.Update;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Mapper.Configurations;
using SharedKernel.Services;
using SharedKernel.Utils;

namespace BadmintonEcommerce.Infrastructure.Persistence.Profiles.Catalog.Requests;

public class ProductProfile : IMappingProfile
{
    public void Configure(MapperConfiguration configuration)
    {
        configuration.CreateMap<CreateProductRequest, CreateProductCommand>()
            .ForMember(des => des.ProductName,
                src => src.ProductName)
            .ForMember(des => des.Brand,
                src => src.Brand)
            .ForMember(des => des.CategoryId,
                src => src.CategoryId)
            .ForMember(des => des.Description,
                src => src.ProductDescription);

        configuration.CreateMap<CreateProductCommand, Product>()
            .ForMember(des => des.Description,
                src => src.Description)
            .ForMember(des => des.Brand,
                src => src.Brand)
            .ForMember(des => des.CategoryId,
                src => src.CategoryId)
            .ForMember(des => des.Name,
                src => src.ProductName)
            .ForMember(des => des.Slug,
                src => SlugGenerateProvider.GenerateSlug(src.ProductName));

        configuration.CreateMap<UpdateProductRequest, UpdateProductCommand>()
            .ForMember(des => des.ProductName,
                src => src.ProductName)
            .ForMember(des => des.Brand,
                src => src.Brand)
            .ForMember(des => des.CategoryId,
                src => src.CategoryId)
            .ForMember(des => des.ProductDescription,
                src => src.ProductDescription)
            .ForMember(des => des.Id,
                src => src.Id);

    }
}