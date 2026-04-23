using System.Security.Claims;
using BadmintonEcommerce.Infrastructure.Abstractions;

namespace BadmintonEcommerce.Infrastructure.Authentication;

public static class ClaimsPrincipalExtension
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userId, out Guid parsedUserId)
            ? parsedUserId
            : throw new ApplicationException(Message.Authentication.ClaimsPrincipalMessage.UserIdIsNotAvailable);
    }
}