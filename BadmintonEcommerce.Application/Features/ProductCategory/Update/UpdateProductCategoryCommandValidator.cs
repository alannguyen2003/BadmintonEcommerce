using FluentValidation;

namespace BadmintonEcommerce.Application.Features.ProductCategory.Update;

public class UpdateProductCategoryCommandValidator : AbstractValidator<UpdateProductCategoryCommand>
{
    public UpdateProductCategoryCommandValidator()
    {
        RuleFor(rule => rule.Id)
            .NotEmpty()
            .NotNull();

        RuleFor(rule => rule.CategoryName)
            .NotEmpty()
            .NotNull()
            .MinimumLength(4)
            .MaximumLength(100);
        
        RuleFor(x => x.ParentCategoryId)
            .NotEqual(Guid.Empty)
            .When(x => x.ParentCategoryId.HasValue)
            .WithMessage("ParentCategoryId cannot be empty GUID.");
    }
}