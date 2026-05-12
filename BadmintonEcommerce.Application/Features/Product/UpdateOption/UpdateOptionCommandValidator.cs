using BadmintonEcommerce.Application.Features.Product.CreateProduct;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;
using FluentValidation;

namespace BadmintonEcommerce.Application.Features.Product.UpdateOption;

public class UpdateOptionCommandValidator : AbstractValidator<UpdateOptionCommand>
{
    public UpdateOptionCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required.");

        RuleFor(x => x.AddedOptions)
            .NotNull()
            .WithMessage("AddedOptions cannot be null.");

        RuleForEach(x => x.AddedOptions)
            .SetValidator(new CreateOptionRequestValidator());

        RuleFor(x => x.AddedVariants)
            .NotNull()
            .WithMessage("AddedVariants cannot be null.");

        RuleForEach(x => x.AddedVariants)
            .SetValidator(new CreateVariantRequestValidator());

        RuleFor(x => x.DeletedOptions)
            .NotNull()
            .WithMessage("DeletedOptions cannot be null.");

        RuleForEach(x => x.DeletedOptions)
            .NotEmpty()
            .WithMessage("Deleted option id cannot be empty.");

        RuleFor(x => x.DeletedVariants)
            .NotNull()
            .WithMessage("DeletedVariants cannot be null.");

        RuleForEach(x => x.DeletedVariants)
            .NotEmpty()
            .WithMessage("Deleted variant id cannot be empty.");

        RuleFor(x => x)
            .Must(HaveAtLeastOneOperation)
            .WithMessage(
                "At least one add/delete operation is required.");

        RuleFor(x => x.AddedOptions)
            .Must(HaveNoDuplicateOptionCodes)
            .When(x => x.AddedOptions is not null &&
                       x.AddedOptions.Any())
            .WithMessage(
                "Duplicate option codes are not allowed.");

        RuleFor(x => x.DeletedOptions)
            .Must(HaveNoDuplicateGuids)
            .When(x => x.DeletedOptions is not null)
            .WithMessage(
                "Duplicate deleted option ids are not allowed.");

        RuleFor(x => x.DeletedVariants)
            .Must(HaveNoDuplicateGuids)
            .When(x => x.DeletedVariants is not null)
            .WithMessage(
                "Duplicate deleted variant ids are not allowed.");
    }
    
    private static bool HaveAtLeastOneOperation(
        UpdateOptionCommand command)
    {
        return
            (command.AddedOptions?.Any() ?? false) ||
            (command.AddedVariants?.Any() ?? false) ||
            (command.DeletedOptions?.Any() ?? false) ||
            (command.DeletedVariants?.Any() ?? false);
    }

    private static bool HaveNoDuplicateOptionCodes(
        List<CreateOptionRequest> options)
    {
        return options
            .GroupBy(x => x.Code.ToLower())
            .All(x => x.Count() == 1);
    }

    private static bool HaveNoDuplicateGuids(
        List<Guid> ids)
    {
        return ids
            .Distinct()
            .Count() == ids.Count;
    }
}