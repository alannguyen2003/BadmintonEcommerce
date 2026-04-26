using BadmintonEcommerce.Domain.Abstraction.Errors;
using SharedKernel.Errors;

namespace BadmintonEcommerce.Domain.Errors;

public class ProductCategoryError
{
    public static Error NotFound(Guid productCategoryId) => Error.NotFound(
        ProductCategoryErrorCommand.NotFound.Code,
        ProductCategoryErrorCommand.NotFound.Description + productCategoryId);
}