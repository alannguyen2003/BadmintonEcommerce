using BadmintonEcommerce.Domain.Entities.Order;
using BadmintonEcommerce.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonEcommerce.Infrastructure.Persistence.Configurations.OrderConfiguration;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        #region Tables

        builder.ToTable(EntityTypeConfiguration.Table.OrderContext.OrderItemTable);

        #endregion

        #region Primary Keys

        builder.HasKey(o => o.Id);
        
        #endregion

        #region Properties

        builder.Property(b => b.ProvisionalCost)
            .IsRequired()
            .HasDefaultValue(0);
        builder.Property(b => b.Quantity)
            .IsRequired()
            .HasDefaultValue(0);

        #endregion

        #region Foreign Keys

        builder.HasOne(b => b.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(b => b.OrderId);
        builder.HasOne(b => b.Variant)
            .WithMany(b => b.OrderItems)
            .HasForeignKey(b => b.VariantId);

        #endregion
    }
}