using FluentValidation;

namespace BadmintonEcommerce.Application.Features.Product.ChangePrimaryImage;

public sealed class ChangePrimaryImageCommandValidator : AbstractValidator<ChangePrimaryImageCommand>
{
    public ChangePrimaryImageCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product id cannot be empty.");

        RuleFor(x => x.ImageId)
            .NotEmpty()
            .WithMessage("Image id cannot be empty.");
    }
}