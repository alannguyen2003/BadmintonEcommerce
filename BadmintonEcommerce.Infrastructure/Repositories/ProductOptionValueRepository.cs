using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Infrastructure.Persistence.Database;

namespace BadmintonEcommerce.Infrastructure.Repositories;

public class ProductOptionValueRepository(ApplicationDbContext context) 
    : Repository<ProductOptionValue>(context), IProductOptionValueRepository
{
    
}