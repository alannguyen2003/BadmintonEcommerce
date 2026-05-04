using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonEcommerce.Infrastructure.Persistence.Configurations.CatalogConfiguration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        #region Table
        builder.ToTable(EntityTypeConfiguration.Table.CatalogContext.ProductTable);
        #endregion
        
        #region Keys
        builder.HasKey(b => b.Id);
        #endregion
        
        #region Property
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(b => b.Description)
            .IsRequired(false)
            .HasMaxLength(1000);
        builder.Property(b => b.Brand)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(b => b.Slug)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(b => b.Status)
            .IsRequired()
            .HasDefaultValue(false);
        #endregion  
        
        #region Foreign Keys
        builder.HasOne(b => b.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
        #endregion
    }
}