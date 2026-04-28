using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BadmintonEcommerce.Infrastructure.Utils;

public static class SlugGenerateProvider
{
    public static string GenerateSlug(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // 1. Normalize Unicode (FormD tách dấu ra khỏi ký tự)
        string normalized = input.Normalize(NormalizationForm.FormD);

        var sb = new StringBuilder();

        foreach (char c in normalized)
        {
            var unicodeCategory = Char.GetUnicodeCategory(c);

            // 2. Bỏ dấu (NonSpacingMark)
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        string result = sb.ToString().Normalize(NormalizationForm.FormC);

        // 3. Replace 'đ' riêng (vì nó không phải dấu kết hợp)
        result = result.Replace('đ', 'd').Replace('Đ', 'D');

        // 4. To lower
        result = result.ToLowerInvariant();

        // 5. Remove invalid chars
        result = Regex.Replace(result, @"[^a-z0-9\s-]", "");

        // 6. Replace whitespace with dash
        result = Regex.Replace(result, @"\s+", "-");

        // 7. Remove multiple dashes
        result = Regex.Replace(result, @"-+", "-");

        // 8. Trim dash
        result = result.Trim('-');

        return result;
    }
}