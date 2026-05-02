using BadmintonEcommerce.Domain.Entities.Order;
using BadmintonEcommerce.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonEcommerce.Infrastructure.Persistence.Configurations.OrderConfiguration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        #region Tables
        
        builder.ToTable(EntityTypeConfiguration.Table.OrderContext.OrderTable);
        
        #endregion
        
        #region Primary Keys
        
        builder.HasKey(k => k.Id);
        
        #endregion
        
        #region Properties
        
        
        
        #endregion
        
        #region Foreign Keys
        
        
        
        #endregion
    }
}