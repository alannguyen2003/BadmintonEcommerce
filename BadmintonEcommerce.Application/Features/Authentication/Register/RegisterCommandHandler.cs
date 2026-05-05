using BadmintonEcommerce.Application.Abstraction.Authentication;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Abstraction;
using BadmintonEcommerce.Domain.Entities.Authentication;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Application.Features.Authentication.Register;

public class RegisterCommandHandler(
    IAccountRepository accountRepository,
    IRoleRepository roleRepository,
    IPasswordHasher passwordHasher,
    IMapper mapper,
    IDateTimeProvider dateTimeProvider,
    ITokenProvider tokenProvider) : ICommandHandler<RegisterCommand, string>
{
    public async Task<Result<string>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        IEnumerable<Role> roles = await roleRepository.Get();
        //Check if username or email exists
        IEnumerable<Account> accountsQuery = await accountRepository.Get(
            filter: filter => filter.Username.Equals(command.Username) || filter.Email.Equals(command.Email));
        if (accountsQuery.Any()) 
            return Result.Failure<string>(AuthenticationError.EmailExists(command.Email));

        Account account = mapper.Map<Account>(command);
        account.PasswordHashed = passwordHasher.Hash(command.Password);
        account.CreatedOnUtc = dateTimeProvider.UtcNow;
        account.AccountRoles = new List<AccountRole>();
        account.AccountRoles.Add(
            new AccountRole()
            {
                Account = account,
                RoleId = roles.FirstOrDefault(filter => filter.Name.Equals(Roles.User)).Id
            });
        accountRepository.Insert(account);
        await accountRepository.SaveChangesAsync();
        List<Role> accountRoles = account.AccountRoles.Select(item => item.Role).ToList();
        string token = tokenProvider.Create(account, accountRoles.ToList());
        return Result.Success(token);
    }
}