using BadmintonEcommerce.Domain.Abstraction.Errors;
using SharedKernel.Errors;

namespace BadmintonEcommerce.Domain.Errors;

public static class InventoryItemError
{
    public static Error NotFound(Guid inventoryItemId) => Error.NotFound(
        InventoryItemErrorCommand.NotFound.Code,
        InventoryItemErrorCommand.NotFound.Description + inventoryItemId);

    public static Error OutOfQuantity(Guid inventoryItemId) => Error.Problem(
        InventoryItemErrorCommand.OutOfQuantity.Code,
        InventoryItemErrorCommand.OutOfQuantity.Description + inventoryItemId);
}