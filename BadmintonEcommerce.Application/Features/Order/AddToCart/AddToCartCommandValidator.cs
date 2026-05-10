using FluentValidation;

namespace BadmintonEcommerce.Application.Features.Order.AddToCart;

public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
{
    public AddToCartCommandValidator()
    {
        RuleFor(rule => rule.VariantId)
            .NotEqual(Guid.Empty)
            .WithMessage("VariantId cannot be empty GUID.");
        
        RuleFor(rule => rule.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0.");
    }
}