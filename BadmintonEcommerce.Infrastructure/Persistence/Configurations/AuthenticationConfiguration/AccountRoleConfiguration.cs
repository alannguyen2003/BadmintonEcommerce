using BadmintonEcommerce.Domain.Entities.Authentication;
using BadmintonEcommerce.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonEcommerce.Infrastructure.Persistence.Configurations.AuthenticationConfiguration;

public class AccountRoleConfiguration : IEntityTypeConfiguration<AccountRole>
{
    public void Configure(EntityTypeBuilder<AccountRole> builder)
    {
        #region Tables 
        
        builder.ToTable(EntityTypeConfiguration.Table.AuthenticationContext.AccountRoleTable);
        
        #endregion
        
        #region Primary Keys

        builder.HasKey(b => new
        {
            b.AccountId, b.RoleId
        });

        #endregion
        
        #region Foreign Keys
        
        builder.HasOne(b => b.Account)
            .WithMany(b => b.AccountRoles)
            .HasForeignKey(b => b.AccountId);
        
        builder.HasOne(b => b.Role)
            .WithMany(b => b.AccountRoles)
            .HasForeignKey(b => b.RoleId);
        
        #endregion
        
    }
}