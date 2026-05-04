using BadmintonEcommerce.Domain.Abstraction.Errors;
using SharedKernel.Errors;

namespace BadmintonEcommerce.Domain.Errors;

public static class ProductImageError
{
    public static Error NotFound(Guid imageId) => Error.NotFound(
        ProductImageErrorCommand.NotFound.Code,
        ProductImageErrorCommand.NotFound.Description + imageId);
}