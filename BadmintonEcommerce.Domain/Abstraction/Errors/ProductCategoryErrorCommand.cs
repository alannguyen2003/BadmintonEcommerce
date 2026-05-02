using SharedKernel.Errors;

namespace BadmintonEcommerce.Domain.Abstraction.Errors;

public static class ProductCategoryErrorCommand
{
    public static class NotFound
    {
        public const string Code =  "ProductCategory.NotFound";
        public const string Description = "The product category is not found. Id: ";
    }

    public static class CannotCreateMoreThan3Levels
    {
        public const string Code = "ProductCategory.CannotCreateMoreThan3Levels";
        public const string Description = "The product category cannot have more than 3 levels.";
    }
}