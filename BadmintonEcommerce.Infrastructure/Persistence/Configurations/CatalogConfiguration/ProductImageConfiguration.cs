using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonEcommerce.Infrastructure.Persistence.Configurations.CatalogConfiguration;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        #region Table

        builder.ToTable(EntityTypeConfiguration.Table.CatalogContext.ProductImageTable);

        #endregion
        
        #region Primary Key

        builder.HasKey(b => b.Id);
        
        #endregion
        
        #region Properties

        builder.Property(b => b.Url)
            .IsRequired();
        builder.Property(b => b.IsPrimary)
            .IsRequired();

        #endregion

        #region Foreign Keys

        builder.HasOne(b => b.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(b => b.ProductId);

        #endregion

    }
}