using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;

namespace BadmintonEcommerce.Application.Abstraction.Data;

public interface IApplicationDbContext
{
    //Catalog Context
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> Categories { get; set; }
    public DbSet<ProductOption> ProductOptions { get; set; }
    public DbSet<ProductOptionValue> ProductOptionValues { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<VariantCombination> Combinations { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    
    //Inventory Context
    public DbSet<InventoryItem> InventoryItems { get; set; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}