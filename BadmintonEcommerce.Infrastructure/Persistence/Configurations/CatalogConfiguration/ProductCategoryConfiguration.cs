using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonEcommerce.Infrastructure.Persistence.Configurations.CatalogConfiguration;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        #region Table

        builder.ToTable(EntityTypeConfiguration.Table.CatalogContext.ProductCategoryTable);

        #endregion
        
        #region Primary Key

        builder.HasKey(b => b.Id);
        
        #endregion
        
        #region Properties
        
        builder.Property(p => p.CategoryName)
            .IsRequired() 
            .HasMaxLength(100);
        
        builder.Property(b => b.Level)
            .IsRequired()
            .HasDefaultValue(1);
        
        #endregion

        #region Foreign Keys

        builder.HasOne(p => p.ParentCategory)
            .WithMany(p => p.ChildCategories)
            .HasForeignKey(f => f.ParentCategoryId);

        #endregion
    }
}