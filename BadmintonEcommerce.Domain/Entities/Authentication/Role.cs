using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Domain.Entities.Authentication;

public class Role : Entity<Guid>
{
    public string Name { get; set; }
    
    //Collections
    public ICollection<AccountRole> AccountRoles { get; set; }
}