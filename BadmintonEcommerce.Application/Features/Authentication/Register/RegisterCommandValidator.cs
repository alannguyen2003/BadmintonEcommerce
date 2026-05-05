using FluentValidation;

namespace BadmintonEcommerce.Application.Features.Authentication.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(rule => rule.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(rule => rule.Fullname)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(rule => rule.Password)
            .MaximumLength(32)
            .MinimumLength(8)
            .NotEmpty();

        RuleFor(rule => rule.Username)
            .NotEmpty()
            .MaximumLength(32)
            .MinimumLength(4);
    }
}