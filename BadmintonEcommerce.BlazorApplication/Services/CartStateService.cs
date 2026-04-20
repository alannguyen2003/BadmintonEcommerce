using BadmintonEcommerce.BlazorApplication.Models;

namespace BadmintonEcommerce.BlazorApplication.Services;

public sealed class CartStateService
{
    private readonly List<CartItem> _items = new();

    public IReadOnlyList<CartItem> Items => _items;

    public int TotalQuantity => _items.Sum(i => i.Quantity);

    public decimal SubtotalBeforeDiscount => _items.Sum(i => i.LineSubtotalBeforeDiscount);

    public decimal DiscountTotal => _items.Sum(i => i.LineSubtotalBeforeDiscount - i.LineSubtotalAfterDiscount);

    public decimal SubtotalAfterDiscount => _items.Sum(i => i.LineSubtotalAfterDiscount);

    public event Action? OnChange;

    public void AddItem(Product product, ProductVariant variant, int quantity = 1)
    {
        if (quantity <= 0) return;
        if (variant.Stock <= 0) return;

        var (before, after) = product.GetPrices(variant);

        var existing = _items.FirstOrDefault(
            x => x.VariantId == variant.Id && x.ProductSlug.Equals(product.Slug, StringComparison.OrdinalIgnoreCase));

        if (existing is null)
        {
            var qty = Math.Min(quantity, Math.Max(0, variant.Stock));
            _items.Add(new CartItem
            {
                ProductId = product.Id,
                ProductSlug = product.Slug,
                ProductName = product.Name,
                VariantId = variant.Id,
                VariantDisplayName = variant.DisplayName,
                VariantAttributes = new Dictionary<string, string>(variant.Attributes),
                UnitPriceBeforeDiscount = before,
                UnitPriceAfterDiscount = after,
                Quantity = qty,
                ImageUrl = string.IsNullOrWhiteSpace(variant.ImageUrl) ? product.HeroImageUrl : variant.ImageUrl
            });
        }
        else
        {
            var maxAllowed = Math.Max(0, variant.Stock);
            existing.Quantity = Math.Min(existing.Quantity + quantity, maxAllowed);
        }

        NotifyChanged();
    }

    public void SetQuantity(string lineId, int quantity)
    {
        var line = _items.FirstOrDefault(x => x.LineId == lineId);
        if (line is null) return;

        if (quantity <= 0)
        {
            _items.Remove(line);
            NotifyChanged();
            return;
        }

        line.Quantity = quantity;
        NotifyChanged();
    }

    public void RemoveLine(string lineId)
    {
        _items.RemoveAll(x => x.LineId == lineId);
        NotifyChanged();
    }

    public void Clear()
    {
        _items.Clear();
        NotifyChanged();
    }

    private void NotifyChanged() => OnChange?.Invoke();
}