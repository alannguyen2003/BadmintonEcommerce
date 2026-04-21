using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonEcommerce.Infrastructure.Persistence.Configurations.CatalogConfiguration;

public class VariantCombinationConfiguration : IEntityTypeConfiguration<VariantCombination>
{
    public void Configure(EntityTypeBuilder<VariantCombination> builder)
    {
        #region Table

        builder.ToTable(EntityTypeConfiguration.Table.CatalogContext.VariantCombinationTable);

        #endregion
        
        #region Primary Keys

        builder.HasKey(b => new { b.OptionValueId, b.VariantId });

        #endregion
        
        #region Foreign Keys 
        
        builder.HasOne(p => p.OptionValue)
            .WithMany(b => b.Combinations)
            .HasForeignKey(p => p.OptionValueId);
        builder.HasOne(p => p.Variant)
            .WithMany(b => b.Combinations)
            .HasForeignKey(p => p.VariantId);
        
        #endregion
    }
}