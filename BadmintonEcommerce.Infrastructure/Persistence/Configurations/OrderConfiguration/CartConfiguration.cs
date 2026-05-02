using BadmintonEcommerce.Domain.Entities.Order;
using BadmintonEcommerce.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonEcommerce.Infrastructure.Persistence.Configurations.OrderConfiguration;

public class CartConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        #region Tables
        
        builder.ToTable(EntityTypeConfiguration.Table.OrderContext.CartTable);
        
        #endregion

        #region Primary Keys

        builder.HasKey(k => k.Id);

        #endregion
        
        #region Properties
        
        builder.Property(k => k.Quantity)
            .IsRequired()
            .HasDefaultValue(1);
        builder.Property(b => b.ProvisionalPrice)
            .IsRequired()
            .HasDefaultValue(0);
        
        #endregion
        
        #region Foreign Keys

        builder.HasOne(b => b.Variant)
            .WithMany(b => b.CartItems)
            .HasForeignKey(b => b.VariantId);

        #endregion
    }
}