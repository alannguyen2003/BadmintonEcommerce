using FluentValidation;

namespace BadmintonEcommerce.Application.Features.Product.CreateOption;

public class CreateProductOptionValueCommandValidator
    : AbstractValidator<CreateProductOptionValueCommand>
{
    public CreateProductOptionValueCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required.");

        RuleFor(x => x.OptionName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Option name is required.")
            .MaximumLength(100)
            .WithMessage("Option name cannot exceed 100 characters.");

        RuleFor(x => x.OptionValues)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Option values cannot be null.")
            .Must(x => x.Length > 0)
            .WithMessage("At least one option value is required.");

        RuleForEach(x => x.OptionValues)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Option value cannot be empty.")
            .MaximumLength(100)
            .WithMessage("Option value cannot exceed 100 characters.");

        RuleFor(x => x.OptionValues)
            .Must(HaveNoDuplicateValues)
            .When(x => x.OptionValues is not null && x.OptionValues.Length > 0)
            .WithMessage("Duplicate option values are not allowed.");
    }

    private static bool HaveNoDuplicateValues(string[] values)
    {
        return values
            .Select(x => x.Trim().ToLower())
            .Distinct()
            .Count() == values.Length;
    }
}