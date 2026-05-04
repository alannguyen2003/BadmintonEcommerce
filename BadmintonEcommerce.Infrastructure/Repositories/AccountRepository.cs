using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Entities.Authentication;
using BadmintonEcommerce.Infrastructure.Persistence.Database;

namespace BadmintonEcommerce.Infrastructure.Repositories;

public class AccountRepository(ApplicationDbContext context) 
    : Repository<Account>(context), IAccountRepository
{
    
}