using System.Security.Claims;
using BadmintonEcommerce.Application.Abstraction.Authentication;
using Microsoft.AspNetCore.Http;

namespace BadmintonEcommerce.Infrastructure.Authentication;

public class AccountContext : IAccountContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid AccountId
    {
        get
        {
            var claims = _httpContextAccessor.HttpContext.User.FindFirstValue("sub");
            return _httpContextAccessor
                       .HttpContext?
                       .User
                       .GetAccountId() ??
                   throw new UserContextUnavailableException();
        }
    }
}