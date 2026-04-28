using FluentValidation;

namespace BadmintonEcommerce.Application.Features.Product.Create;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(rule => rule.ProductName)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(rule => rule.Description)
            .MaximumLength(1000);
        RuleFor(rule => rule.Brand)
            .NotEmpty()
            .MaximumLength(100);
    }
}