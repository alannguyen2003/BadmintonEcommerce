using BadmintonEcommerce.Domain.Abstraction.Errors;
using SharedKernel.Errors;

namespace BadmintonEcommerce.Domain.Errors;

public class ProductCategoryError
{
    public static Error NotFound(Guid productCategoryId) => Error.NotFound(
        ProductCategoryErrorCommand.NotFound.Code,
        ProductCategoryErrorCommand.NotFound.Description + productCategoryId);

    public static Error CannotCreateMoreThan3Levels() => Error.Failure(
        ProductCategoryErrorCommand.CannotCreateMoreThan3Levels.Code,
        ProductCategoryErrorCommand.CannotCreateMoreThan3Levels.Description);
}