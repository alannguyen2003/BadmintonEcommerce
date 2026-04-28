using FluentValidation;

namespace BadmintonEcommerce.Application.Features.Product.Update;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(rule => rule.ProductName)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(rule => rule.ProductDescription)
            .MaximumLength(1000);
        RuleFor(rule => rule.Brand)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(rule => rule.Id)
            .NotEmpty();
        RuleFor(rule => rule.CategoryId)
            .NotEmpty();
    }
}