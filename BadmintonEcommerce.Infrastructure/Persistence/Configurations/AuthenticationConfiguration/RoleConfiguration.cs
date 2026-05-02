using BadmintonEcommerce.Domain.Entities.Authentication;
using BadmintonEcommerce.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonEcommerce.Infrastructure.Persistence.Configurations.AuthenticationConfiguration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        #region Tables

        builder.ToTable(EntityTypeConfiguration.Table.AuthenticationContext.RoleTable);

        #endregion
        
        #region Primary Keys
        
        builder.HasKey(r => r.Id);
        
        #endregion
        
        #region Properties 
        
        builder.Property(r => r.Name)
            .HasMaxLength(256)
            .IsRequired();
        
        #endregion
        
        
    }
}