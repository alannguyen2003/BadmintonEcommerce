using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonEcommerce.Infrastructure.Persistence.Configurations.CatalogConfiguration;

public class ProductOptionValueConfiguration : IEntityTypeConfiguration<ProductOptionValue>
{
    public void Configure(EntityTypeBuilder<ProductOptionValue> builder)
    {
        #region Table

        builder.ToTable(EntityTypeConfiguration.Table.CatalogContext.ProductOptionValueTable);

        #endregion
        
        #region Primary Key

        builder.HasKey(b => b.Id);
        
        #endregion
        
        #region Properties
        
        
        
        #endregion
        
        #region Foreign Keys
        
        builder.HasOne(b => b.Option)
            .WithMany(o => o.OptionValues)
            .HasForeignKey(b => b.OptionId);
        
        #endregion
    }
}