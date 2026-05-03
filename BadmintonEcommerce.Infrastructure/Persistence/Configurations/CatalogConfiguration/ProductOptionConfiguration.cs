using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonEcommerce.Infrastructure.Persistence.Configurations.CatalogConfiguration;

public class ProductOptionConfiguration : IEntityTypeConfiguration<ProductOption>
{
    public void Configure(EntityTypeBuilder<ProductOption> builder)
    {
        #region Table

        builder.ToTable(EntityTypeConfiguration.Table.CatalogContext.ProductOptionTable);
        #endregion
        
        #region Keys
        builder.HasKey(b => b.Id);
        #endregion
        
        #region Properties

        builder.Property(b => b.OptionName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(b => b.Code)
            .IsRequired()
            .HasMaxLength(100);

        #endregion
        
        #region Foreign Keys

        builder.HasOne(b => b.Product)
            .WithMany(p => p.Options)
            .HasForeignKey(b => b.ProductId);

        #endregion
    }
}