using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Domain.Entities.Authentication;

public class Account : Aggregate<Guid>
{
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHashed { get; set; }
    
    //Collections
    public ICollection<AccountRole> AccountRoles { get; set; }
}