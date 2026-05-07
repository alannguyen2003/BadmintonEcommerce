namespace BadmintonEcommerce.BlazorApplication.Abstraction.Services;

public interface IStorageService
{
    public void StoreCookie(string accessToken, string author);
    public string? GetAccessToken();
    public string? GetAuthor();

    public void ClearCookies();
    
    public bool CheckIfAuthenticated { get; }
}