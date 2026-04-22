using BadmintonEcommerce.Domain.Entities.Inventory;
using BadmintonEcommerce.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonEcommerce.Infrastructure.Persistence.Configurations.InventoryConfiguration;

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        #region Table

        builder.ToTable(EntityTypeConfiguration.Table.InventoryContext.InventoryItemTable);
        
        #endregion
        
        #region Primary Keys

        builder.HasKey(k => k.Id);

        #endregion
        
        #region Properties

        builder.Property(b => b.Quantity)
            .IsRequired()
            .HasDefaultValue(0);
        
        builder.Property(b => b.Reserved)
            .IsRequired()
            .HasDefaultValue(0);

        #endregion

        #region Foreign Keys

        builder.HasOne(b => b.Variant)
            .WithOne(p => p.Inventory)
            .HasForeignKey<InventoryItem>(b => b.VariantId);
        
        #endregion
    }
}