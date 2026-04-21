using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonEcommerce.Infrastructure.Persistence.Configurations.CatalogConfiguration;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        #region Table
        builder.ToTable(EntityTypeConfiguration.Table.CatalogContext.ProductVariantTable);
        #endregion
        
        #region Key
        builder.HasKey(pv => pv.Id);
        #endregion
        
        #region Properties
        builder.Property(pv => pv.SKU)
            .IsRequired()
            .HasMaxLength(128);
        builder.Property(pv => pv.Price)
            .IsRequired();
        #endregion
    }
}