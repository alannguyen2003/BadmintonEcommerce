using FluentValidation;

namespace BadmintonEcommerce.Application.Features.ProductCategory.Create;

public class CreateProductCategoryCommandValidator 
    : AbstractValidator<CreateProductCategoryCommand>
{
    public CreateProductCategoryCommandValidator()
    {
        RuleFor(rule => rule.CategoryName)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);
        
        RuleFor(x => x.ParentCategoryId)
            .NotEqual(Guid.Empty)
            .When(x => x.ParentCategoryId.HasValue)
            .WithMessage("ParentCategoryId cannot be empty GUID.");
    }
}