using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Entities.Authentication;
using BadmintonEcommerce.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace BadmintonEcommerce.Infrastructure.Repositories;

public class AccountRepository(ApplicationDbContext context) 
    : Repository<Account>(context), IAccountRepository
{
    public async Task<List<Role>> GetAccountRoles(Guid accountId)
    {
        var roles = await context.AccountRoles
            .Where(item => item.AccountId.Equals(accountId))
            .Include(item => item.Role)
            .Select(item => item.Role)
            .ToListAsync();
        return roles;
    }
}