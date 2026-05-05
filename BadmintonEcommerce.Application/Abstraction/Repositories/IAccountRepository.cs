using BadmintonEcommerce.Domain.Entities.Authentication;

namespace BadmintonEcommerce.Application.Abstraction.Repositories;

public interface IAccountRepository : IRepository<Account>
{
    public Task<List<Role>> GetAccountRoles(Guid accountId);
}