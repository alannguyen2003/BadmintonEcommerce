using BadmintonEcommerce.Domain.Entities.Authentication;
using BadmintonEcommerce.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonEcommerce.Infrastructure.Persistence.Configurations.AuthenticationConfiguration;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        #region Table

        builder.ToTable(EntityTypeConfiguration.Table.AuthenticationContext.AccountTable);

        #endregion
        
        #region Primary Keys
        
        builder.HasKey(b => b.Id);
        
        #endregion
        
        #region Properties
        
        builder.Property(b => b.Username)
            .IsRequired()
            .HasMaxLength(128);
        builder.Property(b => b.Email)
            .IsRequired()
            .HasMaxLength(128);
        builder.Property(b => b.FullName)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(b => b.PasswordHashed)
            .IsRequired();

        #endregion
        
        #region Foreign Keys
        
        
        
        #endregion
        
    }
}