using BadmintonEcommerce.Domain.Entities.Authentication;

namespace BadmintonEcommerce.Application.Abstraction.Authentication;

public interface ITokenProvider
{
    string Create(Account account, List<Role> roles);
}