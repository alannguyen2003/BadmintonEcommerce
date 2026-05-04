using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Entities.Authentication;
using BadmintonEcommerce.Infrastructure.Persistence.Database;

namespace BadmintonEcommerce.Infrastructure.Repositories;

public class RoleRepository(ApplicationDbContext context)
    : Repository<Role>(context), IRoleRepository
{
    
}