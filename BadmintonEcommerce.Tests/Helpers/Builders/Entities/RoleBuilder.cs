using BadmintonEcommerce.Domain.Entities.Authentication;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Entities;

public class RoleBuilder
{
    private Guid id;
    private string name;

    public RoleBuilder WithId(Guid id)
    {
        this.id = id;
        return this;
    }

    public RoleBuilder WithName(string name)
    {
        this.name = name;
        return this;
    }

    public Role Build() => new Role()
    {
        Id = this.id,
        Name = this.name
    };
}