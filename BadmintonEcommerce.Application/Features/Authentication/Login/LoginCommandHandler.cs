using BadmintonEcommerce.Application.Abstraction.Authentication;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Contracts.API.Presentation.Authentication.Response;
using BadmintonEcommerce.Domain.Entities.Authentication;
using BadmintonEcommerce.Domain.Errors;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.Authentication.Login;

public class LoginCommandHandler(
    IAccountRepository accountRepository,
    ITokenProvider tokenProvider,
    IPasswordHasher passwordHasher
    ) : ICommandHandler<LoginCommand, string>
{
    public async Task<Result<string>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        IEnumerable<Account> accountsQuery = await accountRepository.Get(

            filter: filter => filter.Email.Equals(command.Email),
            orderBy: null,
            includeProperties: "AccountRoles");
        if (!accountsQuery.Any())
            return Result.Failure<string>(AuthenticationError.EmailNotExists(command.Email));

        bool verified = passwordHasher.Verify(command.Password, accountsQuery.First().PasswordHashed);

        if (!verified)
            return Result.Failure<string>(
                AuthenticationError.EmailOrPasswordIsWrong());
        
        Account? account = accountsQuery.First();
        
        string token = tokenProvider.Create(account, await accountRepository.GetAccountRoles(account.Id));
        return token;
    }
    
}