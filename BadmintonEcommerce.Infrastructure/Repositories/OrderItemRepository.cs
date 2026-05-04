using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Entities.Order;
using BadmintonEcommerce.Infrastructure.Persistence.Database;

namespace BadmintonEcommerce.Infrastructure.Repositories;

public class OrderItemRepository(ApplicationDbContext context) 
    : Repository<OrderItem>(context), IOrderItemRepository
{
    
}