using System.Security.Claims;
using BadmintonEcommerce.Infrastructure.Abstractions;

namespace BadmintonEcommerce.Infrastructure.Authentication;

public static class ClaimsPrincipalExtension
{
    public static Guid GetAccountId(this ClaimsPrincipal? principal)
    {
        string? accountId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(accountId, out Guid parsedAccountId)
            ? parsedAccountId
            : throw new ApplicationException(Message.Authentication.ClaimsPrincipalMessage.AccountIdIsNotAvailable);
    }
}