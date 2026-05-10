using BadmintonEcommerce.Domain.Entities.Authentication;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Entities;

public class AccountBuilder
{
    private Guid id;
    private string email;
    private string passwordHashed;
    private string fullname;

    public AccountBuilder WithId(Guid id)
    {
        this.id = id;
        return this;
    }

    public AccountBuilder WithEmail(string email)
    {
        this.email = email;
        return this;
    }

    public AccountBuilder WithPasswordHashed(string password)
    {
        this.passwordHashed = password;
        return this;
    }

    public AccountBuilder WithFullname(string fullname)
    {
        this.fullname = fullname;
        return this;
    }

    public Account Build() => new Account()
    {
        Id = this.id,
        Email = this.email,
        PasswordHashed = this.passwordHashed,
        FullName = this.fullname
    };
}