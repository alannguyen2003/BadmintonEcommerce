using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Enums;
using BadmintonEcommerce.Infrastructure.Persistence.Abstraction;
using SharedKernel.Services;
using SharedKernel.Utils;

namespace BadmintonEcommerce.Infrastructure.Persistence.Data;

public class CatalogData(IDateTimeProvider dateTimeProvider)
{
    public List<ProductCategory> RootCategories = new List<ProductCategory>()
    {
        new ProductCategory()
        {
            CategoryName = CatalogPreData.Category.Root.Badminton,
            Level = 1,
            ChildCategories =  new List<ProductCategory>()
            {
                new ProductCategory()
                {
                    CategoryName = CatalogPreData.Category.ChildCategory.BadmintonRacquets,
                    Level = 2,
                },
                new ProductCategory() 
                {
                    CategoryName = CatalogPreData.Category.ChildCategory.BadmintonShoes,
                    Level = 2,
                },
                new ProductCategory()
                {
                    CategoryName = CatalogPreData.Category.ChildCategory.BadmintonAccessories,
                    Level = 2,
                },
                new ProductCategory()
                {
                    CategoryName = CatalogPreData.Category.ChildCategory.BadmintonApparel,
                    Level = 2,
                },
                new ProductCategory()
                {
                    CategoryName = CatalogPreData.Category.ChildCategory.BadmintonShuttlecocks,
                    Level = 2,
                },
                new ProductCategory()
                {
                    CategoryName = CatalogPreData.Category.ChildCategory.BadmintonStrings,
                    Level = 2,
                }
            }
        }
    };

    public const string DefaultDataType = "string";

    public List<ProductOption> Options = new List<ProductOption>()
    {
        //Apparel
        new ProductOption()
        {
            Code = SlugGenerateProvider.GenerateSlug(CatalogPreData.Product.Option.ApparelOption.Size),
            DataType = OptionValueDataType.String,
            OptionName = CatalogPreData.Product.Option.ApparelOption.Size,
            CreatedOnUtc = dateTimeProvider.UtcNow,
            OptionValues = new List<ProductOptionValue>()
        },
        //Racquet
        new ProductOption()
        {
            Code = SlugGenerateProvider.GenerateSlug(CatalogPreData.Product.Option.RacquetOption.Grip),
            DataType = OptionValueDataType.String,
            OptionName = CatalogPreData.Product.Option.RacquetOption.Grip,
            CreatedOnUtc = dateTimeProvider.UtcNow,
            OptionValues = new List<ProductOptionValue>()
        },
        new ProductOption()
        {
            Code = SlugGenerateProvider.GenerateSlug(CatalogPreData.Product.Option.RacquetOption.Weight),
            DataType = OptionValueDataType.String,
            OptionName = CatalogPreData.Product.Option.RacquetOption.Weight,
            CreatedOnUtc = dateTimeProvider.UtcNow,
            OptionValues = new List<ProductOptionValue>()
        },
        //Shoe
        new ProductOption()
        {
            Code = SlugGenerateProvider.GenerateSlug(CatalogPreData.Product.Option.ShoeOption.Size),
            DataType = OptionValueDataType.String,
            OptionName = CatalogPreData.Product.Option.ShoeOption.Size,
            CreatedOnUtc = dateTimeProvider.UtcNow,
            OptionValues = new List<ProductOptionValue>()
        },
        new ProductOption()
        {
            Code = SlugGenerateProvider.GenerateSlug(CatalogPreData.Product.Option.ShoeOption.Model),
            DataType = OptionValueDataType.String,
            OptionName = CatalogPreData.Product.Option.ShoeOption.Model,
            CreatedOnUtc = dateTimeProvider.UtcNow,
            OptionValues = new List<ProductOptionValue>()
        }
    };
    
    
    
    
}