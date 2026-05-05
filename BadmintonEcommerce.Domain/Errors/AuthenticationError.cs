using BadmintonEcommerce.Domain.Abstraction.Errors;
using SharedKernel.Errors;

namespace BadmintonEcommerce.Domain.Errors;

public static class AuthenticationError
{
    public static Error EmailExists(string email) => Error.Problem(
        AuthenticationErrorCommand.EmailNotUnique.Code,
        AuthenticationErrorCommand.EmailNotUnique.Description + email);

    public static Error EmailNotExists(string email) => Error.Problem(
        AuthenticationErrorCommand.EmailNotExists.Code,
        AuthenticationErrorCommand.EmailNotExists.Description + email);
}