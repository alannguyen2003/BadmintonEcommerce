using FluentValidation;

namespace BadmintonEcommerce.Application.Features.ProductCategory.Create;

public class CreateProductCategoryCommandValidator 
    : AbstractValidator<CreateProductCategoryCommand>
{
    public CreateProductCategoryCommandValidator()
    {
        RuleFor(rule => rule.CategoryName)
            .NotEmpty()
            .MaximumLength(100);
    }
}