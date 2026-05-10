using FluentValidation;

namespace BadmintonEcommerce.Application.Features.Authentication.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(rule => rule.Email)
            .EmailAddress()
            .NotNull()
            .NotEmpty();

        RuleFor(rule => rule.Password)
            .NotEmpty()
            .NotNull()
            .MaximumLength(32)
            .MinimumLength(8);
    }
}