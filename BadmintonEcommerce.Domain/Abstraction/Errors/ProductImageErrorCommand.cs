namespace BadmintonEcommerce.Domain.Abstraction.Errors;

public class ProductImageErrorCommand
{
    public static class NotFound
    {
        public const string Code = "ProductImage.NotFound";
        public const string Description = "The product image is not found! Id: ";
    }
}