using BadmintonEcommerce.Contracts.API.Presentation.File.Request;
using FluentValidation;

namespace BadmintonEcommerce.Application.Features.Product.UpdateProduct;

public class UpdateFullProductCommandValidator : AbstractValidator<UpdateFullProductCommand>
{
    public UpdateFullProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product id is required.");

        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Product name is required.")
            .MaximumLength(200)
            .WithMessage("Product name cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Product description is required.")
            .MaximumLength(4000)
            .WithMessage("Product description cannot exceed 4000 characters.");

        RuleFor(x => x.Brand)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Brand is required.")
            .MaximumLength(100)
            .WithMessage("Brand cannot exceed 100 characters.");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category id is required.");

        RuleFor(x => x.DeletedImages)
            .NotNull()
            .WithMessage("Deleted images cannot be null.");

        RuleForEach(x => x.DeletedImages)
            .NotEmpty()
            .WithMessage("Deleted image id cannot be empty.");

        RuleFor(x => x.AddedImages)
            .NotNull()
            .WithMessage("Added images cannot be null.");

        RuleForEach(x => x.AddedImages)
            .Must(BeValidFile)
            .WithMessage("Invalid uploaded file.");

        RuleFor(x => x.UpdatedOptions)
            .NotNull()
            .WithMessage("Updated options cannot be null.");

        RuleForEach(x => x.UpdatedOptions)
            .SetValidator(new UpdateOptionValidator());

        RuleFor(x => x.DeletedOptions)
            .NotNull()
            .WithMessage("Deleted options cannot be null.");

        RuleForEach(x => x.DeletedOptions)
            .NotEmpty()
            .WithMessage("Deleted option id cannot be empty.");

        RuleFor(x => x.DeletedOptionValues)
            .NotNull()
            .WithMessage("Deleted option values cannot be null.");

        RuleForEach(x => x.DeletedOptionValues)
            .NotEmpty()
            .WithMessage("Deleted option value id cannot be empty.");

        RuleFor(x => x.UpdatedOptionValues)
            .NotNull()
            .WithMessage("Updated option values cannot be null.");

        RuleForEach(x => x.UpdatedOptionValues)
            .SetValidator(new UpdateOptionValueValidator());

        RuleFor(x => x.UpdatedOptions)
            .Must(HaveNoDuplicateOptionIds)
            .When(x => x.UpdatedOptions is not null)
            .WithMessage("Duplicate updated option ids are not allowed.");

        RuleFor(x => x.UpdatedOptionValues)
            .Must(HaveNoDuplicateOptionValueIds)
            .When(x => x.UpdatedOptionValues is not null)
            .WithMessage("Duplicate updated option value ids are not allowed.");

        RuleFor(x => x.DeletedImages)
            .Must(HaveNoDuplicateGuids)
            .When(x => x.DeletedImages is not null)
            .WithMessage("Duplicate deleted image ids are not allowed.");

        RuleFor(x => x.DeletedOptions)
            .Must(HaveNoDuplicateGuids)
            .When(x => x.DeletedOptions is not null)
            .WithMessage("Duplicate deleted option ids are not allowed.");

        RuleFor(x => x.DeletedOptionValues)
            .Must(HaveNoDuplicateGuids)
            .When(x => x.DeletedOptionValues is not null)
            .WithMessage("Duplicate deleted option value ids are not allowed.");
    }
    
    private static bool BeValidFile(FileUploadStreamData? file)
    {
        return file is not null;
    }

    private static bool HaveNoDuplicateGuids(List<Guid> ids)
    {
        return ids
            .Distinct()
            .Count() == ids.Count;
    }

    private static bool HaveNoDuplicateOptionIds(
        List<UpdateOption> options)
    {
        return options
            .GroupBy(x => x.Id)
            .All(x => x.Count() == 1);
    }

    private static bool HaveNoDuplicateOptionValueIds(
        List<UpdateOptionValue> optionValues)
    {
        return optionValues
            .GroupBy(x => x.Id)
            .All(x => x.Count() == 1);
    }
}

public class UpdateOptionValidator
    : AbstractValidator<UpdateOption>
{
    public UpdateOptionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Option id is required.");

        RuleFor(x => x.OptionName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Option name is required.")
            .MaximumLength(100)
            .WithMessage("Option name cannot exceed 100 characters.");
    }
}

public class UpdateOptionValueValidator
    : AbstractValidator<UpdateOptionValue>
{
    public UpdateOptionValueValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Option value id is required.");

        RuleFor(x => x.OptionId)
            .NotEmpty()
            .WithMessage("Option id is required.");

        RuleFor(x => x.NewValue)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("New value is required.")
            .MaximumLength(100)
            .WithMessage("New value cannot exceed 100 characters.");
    }
}