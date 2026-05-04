using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Entities.Inventory;
using BadmintonEcommerce.Infrastructure.Persistence.Database;

namespace BadmintonEcommerce.Infrastructure.Repositories;

public class InventoryTransactionRepository(ApplicationDbContext context)
    : Repository<InventoryTransaction>(context), IInventoryTransactionRepository
{
    
}