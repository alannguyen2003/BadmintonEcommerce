using BadmintonEcommerce.Domain.Entities.Authentication;

namespace BadmintonEcommerce.API.Extensions.Data;

public static class Roles
{
    public static List<Role> Data = new List<Role>()
    {
        new Role()
        {
            CreatedOnUtc = DateTime.UtcNow,
            Name = Domain.Abstraction.Roles.Admin
        },
        new Role()
        {
            CreatedOnUtc = DateTime.UtcNow,
            Name = Domain.Abstraction.Roles.User
        }
    };
}