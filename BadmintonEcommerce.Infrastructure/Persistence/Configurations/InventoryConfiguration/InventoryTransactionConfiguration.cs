using BadmintonEcommerce.Domain.Entities.Inventory;
using BadmintonEcommerce.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonEcommerce.Infrastructure.Persistence.Configurations.InventoryConfiguration;

public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        #region Table

        builder.ToTable(EntityTypeConfiguration.Table.InventoryContext.InventoryTransactionTable);

        #endregion

        #region Keys

        builder.HasKey(b => b.Id);

        #endregion

        #region Properties

        builder.Property(b => b.Type)
            .IsRequired();
        builder.Property(b => b.Quantity)
            .IsRequired();

        #endregion

        #region Foreign Keys

        

        #endregion
    }
}