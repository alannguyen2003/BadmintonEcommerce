namespace BadmintonEcommerce.Tests.Helpers.Constants;

public static class Catalog
{
    public static class Get
    {
        
    }

    public static class Create
    {
        public const string IdField = "Id";
        public const string NameField = "CategoryName";
        public const string ParentCategoryIdField = "ParentCategoryId";

        public static class CreateValidCategory
        {
            public const string Name = "Racquets";
            public static Guid? ParentCategoryId = null;
        }
    }
}