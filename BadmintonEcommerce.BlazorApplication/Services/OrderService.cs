using BadmintonEcommerce.BlazorApplication.Models;

namespace BadmintonEcommerce.BlazorApplication.Services;

public sealed class OrderService
{
    private readonly Dictionary<string, Order> _orders = new();

    public Order? GetById(string orderId)
        => _orders.TryGetValue(orderId, out var order) ? order : null;

    public string PlaceOrder(
        CustomerInfo customer,
        CartStateService cart,
        decimal shippingCost)
    {
        if (cart.Items.Count == 0)
            throw new InvalidOperationException("Cannot place order with empty cart.");

        var items = cart.Items.Select(ci => new OrderItem
        {
            ProductSlug = ci.ProductSlug,
            ProductName = ci.ProductName,
            VariantId = ci.VariantId,
            VariantDisplayName = ci.VariantDisplayName,
            VariantAttributes = new Dictionary<string, string>(ci.VariantAttributes),
            UnitPriceBeforeDiscount = ci.UnitPriceBeforeDiscount,
            UnitPriceAfterDiscount = ci.UnitPriceAfterDiscount,
            Quantity = ci.Quantity
        }).ToList();

        var subtotalBefore = items.Sum(i => i.UnitPriceBeforeDiscount * i.Quantity);
        var discountTotal = items.Sum(i => (i.UnitPriceBeforeDiscount - i.UnitPriceAfterDiscount) * i.Quantity);
        var subtotalAfter = items.Sum(i => i.UnitPriceAfterDiscount * i.Quantity);

        if (shippingCost < 0) shippingCost = 0;

        var order = new Order
        {
            Customer = customer,
            SubtotalBeforeDiscount = subtotalBefore,
            DiscountTotal = discountTotal,
            TotalAfterDiscountBeforeShipping = subtotalAfter,
            ShippingCost = shippingCost,
            GrandTotal = subtotalAfter + shippingCost,
            Items = items
        };

        _orders[order.Id] = order;
        cart.Clear();
        return order.Id;
    }
}