using FluentValidation;

namespace BadmintonEcommerce.Application.Features.Product.Delete;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(rule => rule.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required.");
    }
}