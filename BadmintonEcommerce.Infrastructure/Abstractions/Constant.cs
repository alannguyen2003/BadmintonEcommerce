namespace BadmintonEcommerce.Infrastructure.Abstractions;

public static class Constant
{
    public static class Connection
    {
        public static class Database
        {
            public const string DefaultConnection = "BadmintonShopDb";
        }
    }
    public static class Queryable
    {
        public static class Default
        {
            public const int DefaultPageSize = 10;
            public const int DefaultPageIndex = 0;
        }
    }
}