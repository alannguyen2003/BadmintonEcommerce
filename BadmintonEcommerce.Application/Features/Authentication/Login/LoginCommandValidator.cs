using FluentValidation;

namespace BadmintonEcommerce.Application.Features.Authentication.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(rule => rule.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(rule => rule.Password)
            .NotEmpty()
            .MaximumLength(32)
            .MinimumLength(8);
    }
}