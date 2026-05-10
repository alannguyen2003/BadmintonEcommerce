using FluentValidation;

namespace BadmintonEcommerce.Application.Features.Authentication.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(rule => rule.Email)
            .EmailAddress()
            .NotEmpty()
            .NotNull();

        RuleFor(rule => rule.Fullname)
            .NotEmpty()
            .NotNull()
            .MaximumLength(200);

        RuleFor(rule => rule.Password)
            .MaximumLength(32)
            .MinimumLength(8)
            .NotEmpty()
            .NotNull();

        RuleFor(rule => rule.Username)
            .NotEmpty()
            .NotNull()
            .MaximumLength(32)
            .MinimumLength(4);
    }
}