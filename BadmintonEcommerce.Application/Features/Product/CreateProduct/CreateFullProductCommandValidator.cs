using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;
using FluentValidation;

namespace BadmintonEcommerce.Application.Features.Product.CreateProduct;

public class CreateFullProductCommandValidator : AbstractValidator<CreateFullProductCommand>
{
    public CreateFullProductCommandValidator()
    {
        RuleFor(rule => rule.ProductName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(rule => rule.ProductDescription)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(4000);

        RuleFor(rule => rule.Brand)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(rule => rule.ProductCategoryId)
            .NotEmpty()
            .WithMessage("Product category is required.");

        RuleFor(rule => rule.OptionRequests)
            .NotNull()
            .WithMessage("Options are required.");

        RuleForEach(rule => rule.OptionRequests)
            .SetValidator(new CreateOptionRequestValidator());

        RuleFor(rule => rule.VariantRequests)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Variants are required.")
            .Must(rule => rule.Count > 0)
            .WithMessage("At least one variant is required or an empty array.");

        RuleForEach(rule => rule.VariantRequests)
            .SetValidator(new CreateVariantRequestValidator());

        RuleFor(rule => rule.Files)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Product images are required.")
            .Must(x => x.Count >= 0)
            .WithMessage("At least one product image is required.");

        RuleForEach(rule => rule.Files)
            .Must(file => file is not null)
            .WithMessage("Invalid file.");
    }
}

public class CreateOptionRequestValidator : AbstractValidator<CreateOptionRequest>
{
    public CreateOptionRequestValidator()
    {
        RuleFor(rule => rule.Code)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);
        
        RuleFor(rule => rule.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(rule => rule.Values)
            .NotNull()
            .Must(rule => rule.Length >= 0)
            .WithMessage("Option must contain at least one value or empty array.");

        RuleForEach(rule => rule.Values)
            .NotEmpty()
            .MaximumLength(100);
    }
}

public class CreateVariantRequestValidator : AbstractValidator<CreateVariantRequest>
{
    public CreateVariantRequestValidator()
    {
        RuleFor(rule => rule.Price)
            .GreaterThan(0);

        RuleFor(rule => rule.Stock)
            .GreaterThanOrEqualTo(0);

        RuleFor(rule => rule.Values)
            .NotNull()
            .Must(x => x.Count > 0)
            .WithMessage("Variant must contain option values.");
    }
}