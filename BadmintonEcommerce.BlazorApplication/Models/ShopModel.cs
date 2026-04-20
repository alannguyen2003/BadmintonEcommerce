namespace BadmintonEcommerce.BlazorApplication.Models;

using System.Globalization;

public sealed class Category
{
    public string Id { get; init; } = Guid.NewGuid().ToString("N");
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
}

public sealed class Product
{
    public string Id { get; init; } = Guid.NewGuid().ToString("N");
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string CategorySlug { get; init; } = string.Empty;
    public string Brand { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    // Discount applies to all variants of the product.
    public decimal? DiscountPercent { get; init; }
    public decimal? DiscountAmount { get; init; }

    public List<ProductVariant> Variants { get; init; } = new();

    public string HeroImageUrl { get; init; } = string.Empty;

    public (decimal UnitPriceBeforeDiscount, decimal UnitPriceAfterDiscount) GetPrices(ProductVariant variant)
    {
        var before = variant.Price;
        var after = before;

        if (DiscountPercent is not null && DiscountPercent > 0)
        {
            after = before - (before * (DiscountPercent.Value / 100m));
        }
        else if (DiscountAmount is not null && DiscountAmount > 0)
        {
            after = before - DiscountAmount.Value;
        }

        if (after < 0) after = 0;
        return (before, after);
    }

    public decimal GetMinPriceAfterDiscount()
    {
        if (Variants.Count == 0) return 0;
        return Variants.Min(v => GetPrices(v).UnitPriceAfterDiscount);
    }
}

public sealed class ProductVariant
{
    public string Id { get; init; } = Guid.NewGuid().ToString("N");
    public string DisplayName { get; init; } = string.Empty;

    // Simple key/value representation for UI filters (e.g. Size=40, Color=Red)
    public Dictionary<string, string> Attributes { get; init; } = new();

    public decimal Price { get; init; }
    public int Stock { get; init; }

    public string ImageUrl { get; init; } = string.Empty;
}

public sealed class CartItem
{
    public string LineId { get; init; } = Guid.NewGuid().ToString("N");

    public string ProductId { get; init; } = string.Empty;
    public string ProductSlug { get; init; } = string.Empty;
    public string ProductName { get; init; } = string.Empty;

    public string VariantId { get; init; } = string.Empty;
    public string VariantDisplayName { get; init; } = string.Empty;

    public Dictionary<string, string> VariantAttributes { get; init; } = new();

    public decimal UnitPriceBeforeDiscount { get; init; }
    public decimal UnitPriceAfterDiscount { get; init; }
    public int Quantity { get; set; }

    public string ImageUrl { get; init; } = string.Empty;

    public decimal LineSubtotalBeforeDiscount => UnitPriceBeforeDiscount * Quantity;
    public decimal LineSubtotalAfterDiscount => UnitPriceAfterDiscount * Quantity;
}

public sealed class CustomerInfo
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public sealed class Order
{
    public string Id { get; init; } = Guid.NewGuid().ToString("N");
    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;

    public CustomerInfo Customer { get; init; } = new();

    public decimal SubtotalBeforeDiscount { get; init; }
    public decimal DiscountTotal { get; init; }
    public decimal TotalAfterDiscountBeforeShipping { get; init; }

    public decimal ShippingCost { get; init; }
    public decimal GrandTotal { get; init; }

    public List<OrderItem> Items { get; init; } = new();
}

public sealed class OrderItem
{
    public string ProductSlug { get; init; } = string.Empty;
    public string ProductName { get; init; } = string.Empty;

    public string VariantId { get; init; } = string.Empty;
    public string VariantDisplayName { get; init; } = string.Empty;
    public Dictionary<string, string> VariantAttributes { get; init; } = new();

    public decimal UnitPriceBeforeDiscount { get; init; }
    public decimal UnitPriceAfterDiscount { get; init; }

    public int Quantity { get; init; }

    public decimal LineSubtotalAfterDiscount => UnitPriceAfterDiscount * Quantity;
}

public static class MoneyFormat
{
    public static string Vnd(decimal amount)
        => amount.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VND";
}

