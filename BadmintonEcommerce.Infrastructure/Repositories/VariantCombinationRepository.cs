using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Infrastructure.Persistence.Database;

namespace BadmintonEcommerce.Infrastructure.Repositories;

public class VariantCombinationRepository(ApplicationDbContext context)
    : Repository<VariantCombination>(context), IVariantCombinationRepository
{
    
}