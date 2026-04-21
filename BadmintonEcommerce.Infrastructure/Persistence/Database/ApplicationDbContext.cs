using BadmintonEcommerce.Application.Abstraction.Data;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Entities.Inventory;
using BadmintonEcommerce.Infrastructure.DomainEvents;
using Microsoft.EntityFrameworkCore;

namespace BadmintonEcommerce.Infrastructure.Persistence.Database;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IDomainEventDispatcher domainEventDispatcher)
    : DbContext(options), IApplicationDbContext
{
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductCategory> Categories { get; set; }
    public virtual DbSet<ProductOption> ProductOptions { get; set; }
    public virtual DbSet<ProductOptionValue> ProductOptionValues { get; set; }
    public virtual DbSet<ProductVariant> ProductVariants { get; set; }
    public virtual DbSet<VariantCombination> Combinations { get; set; }
    public virtual DbSet<ProductImage> ProductImages { get; set; }
    public virtual DbSet<InventoryItem> InventoryItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        int result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }


    /*private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity => 
                List<IDomainEvent> domainEvents = entity.)
    }*/
}