namespace BadmintonEcommerce.Infrastructure.Abstractions;

public static class EntityTypeConfiguration
{
    public static class Table
    {
        public static class CatalogContext
        {
            public const string ProductTable = "Products";
            public const string ProductImageTable = "ProductImages";
            public const string ProductCategoryTable = "ProductCategories";
            public const string ProductOptionTable = "ProductOptions";
            public const string ProductVariantTable = "ProductVariants";
            public const string VariantCombinationTable = "VariantCombinations";
            public const string ProductOptionValueTable = "ProductOptionValues";
        }

        public static class InventoryContext
        {
            public const string InventoryItemTable = "InventoryItems";
            public const string InventoryTransactionTable = "InventoryTransactions";
        }
    }
}