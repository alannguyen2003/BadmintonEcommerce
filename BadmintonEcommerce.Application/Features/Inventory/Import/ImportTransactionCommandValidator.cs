using FluentValidation;

namespace BadmintonEcommerce.Application.Features.Inventory.Import;

public class ImportTransactionCommandValidator : AbstractValidator<ImportTransactionCommand>
{
    public ImportTransactionCommandValidator()
    {
        RuleFor(x => x.InventoryItemId)
            .NotEmpty()
            .WithMessage("Inventory item id cannot be empty.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid inventory transaction type.");

        RuleFor(x => x.Quantity)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .LessThanOrEqualTo(10000);
    }
}