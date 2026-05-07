using BadmintonEcommerce.BlazorApplication.Abstraction.Services;
using BadmintonEcommerce.Contracts.Endpoints;

namespace BadmintonEcommerce.BlazorApplication.Services;

public class CookieStorageService(
    IHttpContextAccessor httpContextAccessor) : IStorageService
{
    private HttpContext Context => httpContextAccessor.HttpContext!;
    public void StoreCookie(string accessToken, string author)
    {
        var secureOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        };

        // Display name cookie does not need to be HTTP-only — it is not a secret
        var displayOptions = new CookieOptions
        {
            HttpOnly = false,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        };

        Context.Response.Cookies.Append(TokenShared.AccessKey, accessToken, secureOptions);
        Context.Response.Cookies.Append(TokenShared.AuthorKey, author, displayOptions);
    }

    public string? GetAccessToken()
    {
        return Context.Request.Cookies[TokenShared.AccessKey];
    }

    public string? GetAuthor()
    {
        return Context.Request.Cookies[TokenShared.AuthorKey];
    }

    public void ClearCookies()
    {
        Context.Response.Cookies.Delete(TokenShared.AccessKey);
        Context.Response.Cookies.Delete(TokenShared.AuthorKey);
    }

    public bool CheckIfAuthenticated => !string.IsNullOrEmpty(GetAccessToken());
}