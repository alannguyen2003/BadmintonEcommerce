using BadmintonEcommerce.Application.Abstraction.Authentication;
using BadmintonEcommerce.Domain.Entities.Authentication;
using SharedKernel.Services;

namespace BadmintonEcommerce.Infrastructure.Persistence.Data;

public class AuthenticationData(IPasswordHasher passwordHasher, IDateTimeProvider dateTimeProvider)
{
    public List<Account> Accounts = new List<Account>()
    {
        new Account()
        {
            Email = "admin@gmail.com",
            FullName = "Admin",
            PasswordHashed = passwordHasher.Hash("12345678"),
            Username = "admin",
            CreatedOnUtc = dateTimeProvider.UtcNow,
            AccountRoles = new List<AccountRole>()
        },
        new Account
        {
            FullName = "Nguyen Van A",
            Username = "nguyenvana",
            Email = "vana@gmail.com",
            PasswordHashed = passwordHasher.Hash("12345678"),
            CreatedOnUtc = dateTimeProvider.UtcNow,
            AccountRoles = new List<AccountRole>()
        },
        new Account
        {
            FullName = "Tran Thi B",
            Username = "tranthib",
            Email = "thib@gmail.com",
            PasswordHashed = passwordHasher.Hash("12345678"),
            CreatedOnUtc = dateTimeProvider.UtcNow,
            AccountRoles = new List<AccountRole>()
        },
        new Account
        {
            FullName = "Le Van C",
            Username = "levanc",
            Email = "vanc@gmail.com",
            PasswordHashed = passwordHasher.Hash("12345678"),
            CreatedOnUtc = dateTimeProvider.UtcNow,
            AccountRoles = new List<AccountRole>()
        },
        new Account
        {
            FullName = "Pham Thi D",
            Username = "phamthid",
            Email = "thid@gmail.com",
            PasswordHashed = passwordHasher.Hash("12345678"),
            CreatedOnUtc = dateTimeProvider.UtcNow,
            AccountRoles = new List<AccountRole>()
        },
        new Account
        {
            FullName = "Hoang Van E",
            Username = "hoangvane",
            Email = "vane@gmail.com",
            PasswordHashed = passwordHasher.Hash("12345678"),
            CreatedOnUtc = dateTimeProvider.UtcNow,
            AccountRoles = new List<AccountRole>()
        }
    };

    public List<Role> Roles = new List<Role>()
    {
        new Role()
        {
            Name = Domain.Abstraction.Roles.Admin,
            CreatedOnUtc = dateTimeProvider.UtcNow,
            AccountRoles = new List<AccountRole>()
        },
        new Role()
        {
            Name = Domain.Abstraction.Roles.User,
            CreatedOnUtc = dateTimeProvider.UtcNow,
            AccountRoles = new List<AccountRole>()
        }
    };

    public (List<Account>, List<Role>) Data()
    {
        List<Account> accounts = Accounts;
        List<Role> roles = Roles;
        Role adminRole = roles.First(item => item.Name.Equals(Domain.Abstraction.Roles.Admin));
        Role userRole = roles.First(item => item.Name.Equals(Domain.Abstraction.Roles.User));
        foreach (var item in accounts)
        {
            switch (item.Username)
            {
                case "admin":
                    item.AccountRoles.Add(new AccountRole()
                    {
                        Account = item,
                        Role = adminRole
                    });
                    break;
                default:
                    item.AccountRoles.Add(new AccountRole()
                    {
                        Account = item,
                        Role = userRole
                    });
                    break;
            }
        }

        return (accounts, new List<Role>()
        {
            adminRole,
            userRole
        });
    }
}