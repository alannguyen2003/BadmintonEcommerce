using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Entities.Inventory;
using BadmintonEcommerce.Domain.Entities.Order;
using BadmintonEcommerce.Domain.Enums;
using BadmintonEcommerce.Domain.ValueObjects;
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
            ChildCategories = new List<ProductCategory>()
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

    public ProductCategory GetCategory(List<ProductCategory> categories, string categoryName)
    {
        return categories.First(item =>
            item.CategoryName.Equals(categoryName));
    }

    public (List<Product>, List<InventoryItem>) Data(List<ProductCategory> categories)
    {
        // get category
        ProductCategory racquet = GetCategory(categories, CatalogPreData.Category.ChildCategory.BadmintonRacquets);
        ProductCategory shoe = GetCategory(categories, CatalogPreData.Category.ChildCategory.BadmintonShoes);
        ProductCategory apparel = GetCategory(categories, CatalogPreData.Category.ChildCategory.BadmintonApparel);
        ProductCategory accessories =
            GetCategory(categories, CatalogPreData.Category.ChildCategory.BadmintonAccessories);
        ProductCategory shuttlecocks =
            GetCategory(categories, CatalogPreData.Category.ChildCategory.BadmintonShuttlecocks);
        ProductCategory strings = GetCategory(categories, CatalogPreData.Category.ChildCategory.BadmintonStrings);

        var now = dateTimeProvider.UtcNow;

        // ===== SOURCE DATA =====

        var racquetNames = new List<string>()
        {
            // Astrox
            CatalogPreData.Product.Name.Racquet.Astrox.Astrox99ProGen2,
            CatalogPreData.Product.Name.Racquet.Astrox.Astrox100TourVA,
            CatalogPreData.Product.Name.Racquet.Astrox.Astrox70,
            CatalogPreData.Product.Name.Racquet.Astrox.Astrox99Game,
            CatalogPreData.Product.Name.Racquet.Astrox.Astrox88DTour,
            CatalogPreData.Product.Name.Racquet.Astrox.Astrox88DPro,
            CatalogPreData.Product.Name.Racquet.Astrox.Astrox88SPro,
            CatalogPreData.Product.Name.Racquet.Astrox.Astrox88STour,
            CatalogPreData.Product.Name.Racquet.Astrox.Astrox88SGame,
            CatalogPreData.Product.Name.Racquet.Astrox.Astrox77Pro,
            CatalogPreData.Product.Name.Racquet.Astrox.Astrox77Tour,
            CatalogPreData.Product.Name.Racquet.Astrox.Astrox77Game,

            // ArcSaber
            CatalogPreData.Product.Name.Racquet.ArcSaber.ArcSaber11Pro,
            CatalogPreData.Product.Name.Racquet.ArcSaber.ArcSaber11Tour,
            CatalogPreData.Product.Name.Racquet.ArcSaber.ArcSaber11Game,
            CatalogPreData.Product.Name.Racquet.ArcSaber.ArcSaber7Pro,
            CatalogPreData.Product.Name.Racquet.ArcSaber.ArcSaber7Tour,
            CatalogPreData.Product.Name.Racquet.ArcSaber.ArcSaber7Game,

            // NanoFlare
            CatalogPreData.Product.Name.Racquet.NanoFlare.NanoFlareSpeed7,
            CatalogPreData.Product.Name.Racquet.NanoFlare.NanoFlare001Feel,
            CatalogPreData.Product.Name.Racquet.NanoFlare.NanoFlare700Pro,
            CatalogPreData.Product.Name.Racquet.NanoFlare.NanoFlare700Tour,
            CatalogPreData.Product.Name.Racquet.NanoFlare.NanoFlare700Game,
            CatalogPreData.Product.Name.Racquet.NanoFlare.NanoFlare700Play,
            CatalogPreData.Product.Name.Racquet.NanoFlare.NanoFlare800Pro,
            CatalogPreData.Product.Name.Racquet.NanoFlare.NanoFlare800Tour,
            CatalogPreData.Product.Name.Racquet.NanoFlare.NanoFlare800Game,
            CatalogPreData.Product.Name.Racquet.NanoFlare.NanoFlare800Play,
            CatalogPreData.Product.Name.Racquet.NanoFlare.NanoFlare1000Z,
            CatalogPreData.Product.Name.Racquet.NanoFlare.NanoFlare1000Tour,
            CatalogPreData.Product.Name.Racquet.NanoFlare.NanoFlare1000Game,
            CatalogPreData.Product.Name.Racquet.NanoFlare.NanoFlareNextAge
        };

        var shoeNames = new List<string>()
        {
            CatalogPreData.Product.Name.Shoe.EclipsonZ3Men,
            CatalogPreData.Product.Name.Shoe.EclipsonZ3Women,
            CatalogPreData.Product.Name.Shoe.ComfortZ3Women,
            CatalogPreData.Product.Name.Shoe.ComfortZ3Men,
            CatalogPreData.Product.Name.Shoe.CascadeDrive,
            CatalogPreData.Product.Name.Shoe.AerusZWomen,
            CatalogPreData.Product.Name.Shoe.AerusZMen,
            CatalogPreData.Product.Name.Shoe.AerusZWide,
            CatalogPreData.Product.Name.Shoe.Cusion88Dial3,
            CatalogPreData.Product.Name.Shoe.Cusion88Dial3Wide,
            CatalogPreData.Product.Name.Shoe.SubaxiaGTWide
        };

        var accessoryNames = new List<string>()
        {
            CatalogPreData.Product.Name.Accessories.TowelGrip,
            CatalogPreData.Product.Name.Accessories.TowelGripReel,
            CatalogPreData.Product.Name.Accessories.AllSportCrewSocks,
            CatalogPreData.Product.Name.Accessories.ElementCrewSocks,
            CatalogPreData.Product.Name.Accessories.PerformanceCap,
            CatalogPreData.Product.Name.Accessories.SmallLogoWristband
        };

        List<ProductVariant> variants = new List<ProductVariant>();

        // ===== FACTORY FUNCTION =====
        Product Create(string name, string brand, ProductCategory category)
        {
            Product product = new Product()
            {
                Name = name,
                Brand = brand,
                Category = category,
                Slug = SlugGenerateProvider.GenerateSlug(name),
                Status = true,
                Description =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
                    "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                    "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip " +
                    "ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse " +
                    "cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt " +
                    "in culpa qui officia deserunt mollit anim id est laborum.",
                CreatedOnUtc = now,
                Options = new List<ProductOption>(),
                Variants = new List<ProductVariant>(),
                Images = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        IsPrimary = true,
                        ImageMetadata = "",
                        Url = "https://res.cloudinary.com/ds8cv8hcq/image/upload/v1777950873/badminton-ecommerce/bc4q7wuafxjgbjlh6vie.webp",
                        CreatedOnUtc = dateTimeProvider.UtcNow,
                    }
                }
            };
            
            // attach options theo category
            if (category == racquet)
                product.Options = BuildRacquetOptions(product);
            else if (category == shoe)
                product.Options = BuildShoeOptions(product);
            else if (category == apparel)
                product.Options = BuildApparelOptions(product);
            else
                product.Options = BuildDefaultOption(product);

            product.Variants = BuildVariants(product);
            variants.AddRange(product.Variants);
            return product;
        }

        // ===== BUILD DATA =====
        var products = new List<Product>();

        products.AddRange(racquetNames.Select(x =>
            Create(x, CatalogPreData.Product.Brand.Yonex, racquet)));

        products.AddRange(shoeNames.Select(x =>
            Create(x, CatalogPreData.Product.Brand.Yonex, shoe)));

        products.AddRange(accessoryNames.Select(x =>
            Create(x, CatalogPreData.Product.Brand.Yonex, accessories)));

        List<InventoryItem> inventories = BuildInventoryItems(variants);
        return (products, inventories);
    }
    
    private ProductOption CreateOption(
        Product product,
        string optionName,
        string code,
        OptionValueDataType dataType,
        IEnumerable<string> values)
    {
        var option = new ProductOption
        {
            OptionName = optionName,
            Code = code,
            DataType = dataType,
            Product = product,
            ProductId = product.Id,
            OptionValues = values.Select(v => new ProductOptionValue
            {
                Value = v,
                Combinations = new List<VariantCombination>()
            }).ToList()
        };

        return option;
    }
    
    private List<ProductOption> BuildRacquetOptions(Product product)
    {
        return new List<ProductOption>
        {
            CreateOption(
                product,
                CatalogPreData.Product.Option.RacquetOption.Grip,
                SlugGenerateProvider.GenerateSlug(CatalogPreData.Product.Option.RacquetOption.Grip),
                OptionValueDataType.String,
                new[]
                {
                    CatalogPreData.Product.Option.RacquetOption.GripValues.GripG5,
                    CatalogPreData.Product.Option.RacquetOption.GripValues.GripG6
                }),

            CreateOption(
                product,
                CatalogPreData.Product.Option.RacquetOption.Weight,
                SlugGenerateProvider.GenerateSlug(CatalogPreData.Product.Option.RacquetOption.Weight),
                OptionValueDataType.String,
                new[]
                {
                    CatalogPreData.Product.Option.RacquetOption.WeightValues.Weight2U,
                    CatalogPreData.Product.Option.RacquetOption.WeightValues.Weight3U,
                    CatalogPreData.Product.Option.RacquetOption.WeightValues.Weight4U,
                    CatalogPreData.Product.Option.RacquetOption.WeightValues.Weight5U
                })
        };
    }
    
    private List<ProductOption> BuildShoeOptions(Product product)
    {
        return new List<ProductOption>
        {
            CreateOption(
                product,
                CatalogPreData.Product.Option.ShoeOption.Size,
                SlugGenerateProvider.GenerateSlug(CatalogPreData.Product.Option.ShoeOption.Size),
                OptionValueDataType.String,
                new[]
                {
                    CatalogPreData.Product.Option.ShoeOption.SizeValues.Size39,
                    CatalogPreData.Product.Option.ShoeOption.SizeValues.Size40,
                    CatalogPreData.Product.Option.ShoeOption.SizeValues.Size41,
                    CatalogPreData.Product.Option.ShoeOption.SizeValues.Size42,
                    CatalogPreData.Product.Option.ShoeOption.SizeValues.Size43,
                    CatalogPreData.Product.Option.ShoeOption.SizeValues.Size44,
                    CatalogPreData.Product.Option.ShoeOption.SizeValues.Size45,
                    CatalogPreData.Product.Option.ShoeOption.SizeValues.Size46
                }),

            CreateOption(
                product,
                CatalogPreData.Product.Option.ShoeOption.Model,
                SlugGenerateProvider.GenerateSlug(CatalogPreData.Product.Option.ShoeOption.Model),
                OptionValueDataType.String,
                new[]
                {
                    CatalogPreData.Product.Option.ShoeOption.ModelValues.AerusZWideNavyBlue,
                    CatalogPreData.Product.Option.ShoeOption.ModelValues.AerusZWideFlashGreen,
                    CatalogPreData.Product.Option.ShoeOption.ModelValues.AerusZWideIndigo,
                    CatalogPreData.Product.Option.ShoeOption.ModelValues.CascadeDriveBlackBlue,
                    CatalogPreData.Product.Option.ShoeOption.ModelValues.ComfortZ3WideOFWTRD,
                    CatalogPreData.Product.Option.ShoeOption.ModelValues.AerusZMenNavyBlue,
                    CatalogPreData.Product.Option.ShoeOption.ModelValues.CascadeDriveGraphite,
                    CatalogPreData.Product.Option.ShoeOption.ModelValues.EclipsonZ3WomenWhitePurple,
                })
        };
    }
    
    private List<ProductOption> BuildApparelOptions(Product product)
    {
        return new List<ProductOption>
        {
            CreateOption(
                product,
                CatalogPreData.Product.Option.ApparelOption.Size,
                SlugGenerateProvider.GenerateSlug(CatalogPreData.Product.Option.ApparelOption.Size),
                OptionValueDataType.String,
                new[]
                {
                    CatalogPreData.Product.Option.ApparelOption.SizeValues.SizeS,
                    CatalogPreData.Product.Option.ApparelOption.SizeValues.SizeM,
                    CatalogPreData.Product.Option.ApparelOption.SizeValues.SizeL,
                    CatalogPreData.Product.Option.ApparelOption.SizeValues.SizeXL,
                    CatalogPreData.Product.Option.ApparelOption.SizeValues.Size2XL,
                    CatalogPreData.Product.Option.ApparelOption.SizeValues.Size3XL,
                    CatalogPreData.Product.Option.ApparelOption.SizeValues.Size4XL,
                })
        };
    }
    
    private List<ProductOption> BuildDefaultOption(Product product)
    {
        return new List<ProductOption>
        {
            CreateOption(
                product,
                CatalogPreData.Product.Option.Default.DefaultOption,
                SlugGenerateProvider.GenerateSlug(CatalogPreData.Product.Option.Default.DefaultOption),
                OptionValueDataType.String,
                new[]
                {
                    CatalogPreData.Product.Option.Default.DefaultOptionValue
                })
        };
    }
    
    private List<List<ProductOptionValue>> CartesianProduct(List<List<ProductOptionValue>> sequences)
    {
        IEnumerable<List<ProductOptionValue>> result = new List<List<ProductOptionValue>> { new() };

        foreach (var seq in sequences)
        {
            result = result.SelectMany(
                acc => seq,
                (acc, item) =>
                {
                    var newList = new List<ProductOptionValue>(acc) { item };
                    return newList;
                });
        }

        return result.Select(x => x.ToList()).ToList();
    }
    
    private List<ProductVariant> BuildVariants(Product product)
    {
        // Lấy tất cả OptionValues theo từng Option
        var optionGroups = product.Options
            .Select(o => o.OptionValues.ToList())
            .ToList();

        // Nếu không có option → return empty
        if (!optionGroups.Any())
            return new List<ProductVariant>();

        var combinations = CartesianProduct(optionGroups);

        var variants = new List<ProductVariant>();

        int index = 1;

        foreach (var combo in combinations)
        {
            var variant = new ProductVariant
            {
                Product = product,
                ProductId = product.Id,
                SKU = GenerateSku(product, combo, index),
                Price = GeneratePrice(product, combo),

                Combinations = new List<VariantCombination>(),
                CartItems = new List<CartItem>(),
                OrderItems = new List<OrderItem>()
            };

            // ===== CREATE COMBINATIONS =====
            foreach (var value in combo)
            {
                variant.Combinations.Add(new VariantCombination
                {
                    Variant = variant,
                    VariantId = variant.Id,
                    OptionValue = value,
                    OptionValueId = value.Id
                });
            }

            variants.Add(variant);
            index++;
        }

        return variants;
    }
    
    private string GenerateSku(Product product, List<ProductOptionValue> values, int index)
    {
        var optionPart = string.Join("-", values.Select(v => v.Value.Replace(" ", "").ToUpper()));

        return $"{product.Slug.ToUpper()}-{optionPart}-{index:D3}";
    }
    
    private decimal GeneratePrice(Product product, List<ProductOptionValue> values)
    {
        decimal basePrice = 100;

        foreach (var v in values)
        {
            if (v.Value.Contains("2U")) basePrice += 20;
            if (v.Value.Contains("3U")) basePrice += 15;
            if (v.Value.Contains("4U")) basePrice += 10;
            if (v.Value.Contains("5U")) basePrice += 5;

            if (v.Value.Contains("G5")) basePrice += 2;
            if (v.Value.Contains("G6")) basePrice += 1;
        }

        return basePrice;
    }

    private List<InventoryItem> BuildInventoryItems(List<ProductVariant> variants)
    {
        List<InventoryItem> inventories = new List<InventoryItem>();

        foreach (ProductVariant item in variants)
        {
            inventories.Add(new InventoryItem()
            {
                Quantity = 10,
                Variant = item,
                CreatedOnUtc = dateTimeProvider.UtcNow,
                Reserved = 0,
                Transactions = new List<InventoryTransaction>()
                {
                    new InventoryTransaction()
                    {
                        Quantity = 10,
                        Type = InventoryTransactionType.Import,  
                    }
                }
            });
        }

        return inventories;
    }
}