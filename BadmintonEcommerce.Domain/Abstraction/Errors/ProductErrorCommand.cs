namespace BadmintonEcommerce.Domain.Abstraction.Errors;

public static class ProductErrorCommand
{
    public static class NotFound
    {
        public const string Code = "Product.NotFound";
        public const string Description = "The product is not found. Id: ";
    }

    public static class Validator
    {
        public static class ProductName
        {
            public const string NotEmpty = "The product's name must not be empty!";
            public const string MaximumLength100 = "The product's name must be less than 100 characters due to policy.";
        }

        public static class ProductDescription
        {
            public const string MaximumLength1000 =
                "The product's description must be less than 1.000 characters due to the policy.";
        }

        public static class ProductBrand
        {
            public const string NotEmpty = "The product's brand must not be empty!";

            public const string MaximumLength100 =
                "The product's brand must be less than 100 characters due to the policy!";
        }

        public static class ProductCategory
        {
            public const string NotEmpty = "The product's category must not be empty, it must be in a category.";
        }

        public static class ProductId
        {
            public const string NotEmpty = "The product's Id must not be empty.";
        }
    }
}