using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Infrastructure.Persistence.Database;

namespace BadmintonEcommerce.Infrastructure.Repositories;

public class ProductCategoryRepository(ApplicationDbContext context) : Repository<ProductCategory>(context), IProductCategoryRepository
{
    
}