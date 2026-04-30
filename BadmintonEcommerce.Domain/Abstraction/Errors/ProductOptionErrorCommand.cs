namespace BadmintonEcommerce.Domain.Abstraction.Errors;

public static class ProductOptionErrorCommand
{
    public static class NotFound
    {
        public const string Code = "ProductOption.NotFound";
        public const string Description = "The product option is not found. Id :";
    }

    public static class NotAvailable
    {
        public const string Code = "ProductOption.NotAvailable";
        public const string Description = "The product option is not available. Id: ";
    }
}