namespace BadmintonEcommerce.Infrastructure.Authorization;

public sealed class PermissionProvider
{
    public Task<HashSet<string>> GetForAccountIdAsync(Guid accountId)
    {
        HashSet<string> permissionsSet = [];

        return Task.FromResult(permissionsSet);
    }
}