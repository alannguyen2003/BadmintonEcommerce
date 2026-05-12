using BadmintonEcommerce.Contracts.API.Presentation.File.Request;
using FluentValidation;

namespace BadmintonEcommerce.Application.Features.Product.UploadProductImage;

public class UploadProductImageCommandValidator : AbstractValidator<UploadProductImageCommand>
{
    public UploadProductImageCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required.");

        RuleFor(x => x.Files)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Files cannot be null.")
            .Must(x => x.Count > 0)
            .WithMessage("At least one image is required.");

        RuleForEach(x => x.Files)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Invalid file.")
            .Must(BeValidFile)
            .WithMessage("File stream is invalid.");
    }

    private static bool BeValidFile(FileUploadStreamData? file)
    {
        if (file is null)
            return false;

        return !string.IsNullOrWhiteSpace(file.FileName)
               && !string.IsNullOrWhiteSpace(file.ContentType)
               && file.Stream is not null;
    }
}