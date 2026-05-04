namespace BadmintonEcommerce.Domain.Abstraction.Errors;

public class InventoryItemErrorCommand
{
    public static class NotFound
    {
        public const string Code = "InventoryItem.NotFound";
        public const string Description = "The inventory item is not found! Id: ";
    }
    
    public static class OutOfQuantity
    {
        public const string Code = "InventoryItem.OutOfQuantity";
        public const string Description = "The inventory item is out of quantity! Id: ";
    }
}