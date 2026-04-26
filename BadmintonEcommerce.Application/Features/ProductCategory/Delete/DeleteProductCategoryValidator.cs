using FluentValidation;

namespace BadmintonEcommerce.Application.Features.ProductCategory.Delete;

public sealed class DeleteProductCategoryValidator : AbstractValidator<DeleteProductCategoryCommand>
{
    public DeleteProductCategoryValidator()
    {
        RuleFor(r => r.ProductCategoryId)
            .NotEmpty()
            .WithMessage("Product Category Id cannot be empty.");
    }
}