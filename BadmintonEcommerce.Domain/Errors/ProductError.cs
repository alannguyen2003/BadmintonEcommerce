using BadmintonEcommerce.Domain.Abstraction.Errors;
using SharedKernel.Errors;

namespace BadmintonEcommerce.Domain.Errors;

public class ProductError
{
    public static Error NotFound(Guid productId) => Error.NotFound(
        ProductErrorCommand.NotFound.Code,
        ProductErrorCommand.NotFound.Description + productId);
}