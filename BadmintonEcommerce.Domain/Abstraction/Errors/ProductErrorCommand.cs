namespace BadmintonEcommerce.Domain.Abstraction.Errors;

public static class ProductErrorCommand
{
    public static class NotFound
    {
        public const string Code = "Product.NotFound";
        public const string Description = "The product is not found. Id: ";
    }
}