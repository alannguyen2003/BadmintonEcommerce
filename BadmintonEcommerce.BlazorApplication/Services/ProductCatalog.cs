using BadmintonEcommerce.BlazorApplication.Models;

namespace BadmintonEcommerce.BlazorApplication.Services;

public sealed class ProductCatalog
{
    private readonly List<Category> _categories;
    private readonly List<Product> _products;

    public ProductCatalog()
    {
        _categories = new List<Category>
        {
            new() { Name = "Vợt Cầu Lông", Slug = "vot" },
            new() { Name = "Giày Cầu Lông", Slug = "giay" },
            new() { Name = "Quả Cầu", Slug = "qua-cau" },
            new() { Name = "Quần Áo", Slug = "quan-ao" },
            new() { Name = "Phụ Kiện", Slug = "phu-kien" }
        };

        _products = new List<Product>
        {
            // VỢT
            new Product
            {
                Name = "Vợt RedComet 5.0",
                Slug = "vot-redcomet-50",
                CategorySlug = "vot",
                Brand = "RedComet",
                Description = "Thiên về tấn công cân bằng - phản hồi tốt, dễ kiểm soát nhịp vung.",
                DiscountPercent = 5,
                HeroImageUrl = PlaceholderSvg("VOT 50", "#ef4444"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "3U - Cứng vừa",
                        Price = 899000,
                        Stock = 28,
                        Attributes = new Dictionary<string, string> { ["Trọng lượng"] = "3U", ["Độ cứng"] = "Trung bình" },
                        ImageUrl = PlaceholderSvg("3U", "#ef4444")
                    },
                    new()
                    {
                        DisplayName = "4U - Cứng khá",
                        Price = 949000,
                        Stock = 14,
                        Attributes = new Dictionary<string, string> { ["Trọng lượng"] = "4U", ["Độ cứng"] = "Khá" },
                        ImageUrl = PlaceholderSvg("4U", "#3b82f6")
                    }
                }
            },
            new Product
            {
                Name = "Vợt BlueNova 2.5",
                Slug = "vot-bluenova-25",
                CategorySlug = "vot",
                Brand = "BlueNova",
                Description = "Trọng lượng nhẹ - hỗ trợ phản xạ nhanh, hợp đánh đơn/đôi thiên kỹ thuật.",
                DiscountPercent = 12,
                HeroImageUrl = PlaceholderSvg("VOT 25", "#3b82f6"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "3U - Cân bằng",
                        Price = 769000,
                        Stock = 20,
                        Attributes = new Dictionary<string, string> { ["Trọng lượng"] = "3U", ["Độ cứng"] = "Trung bình" },
                        ImageUrl = PlaceholderSvg("3U", "#2563eb")
                    },
                    new()
                    {
                        DisplayName = "4U - Linh hoạt",
                        Price = 819000,
                        Stock = 16,
                        Attributes = new Dictionary<string, string> { ["Trọng lượng"] = "4U", ["Độ cứng"] = "Mềm" },
                        ImageUrl = PlaceholderSvg("4U", "#1d4ed8")
                    }
                }
            },
            new Product
            {
                Name = "Vợt ProStrike XT",
                Slug = "vot-prostrike-xt",
                CategorySlug = "vot",
                Brand = "ProStrike",
                Description = "Form cứng - lực trả tốt, phù hợp lối đánh mạnh và kiểm soát tốc độ.",
                HeroImageUrl = PlaceholderSvg("PRO XT", "#111827"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "3U - Cứng",
                        Price = 1099000,
                        Stock = 9,
                        Attributes = new Dictionary<string, string> { ["Trọng lượng"] = "3U", ["Độ cứng"] = "Cứng" },
                        ImageUrl = PlaceholderSvg("3U", "#111827")
                    },
                    new()
                    {
                        DisplayName = "4U - Cứng vừa",
                        Price = 1049000,
                        Stock = 12,
                        Attributes = new Dictionary<string, string> { ["Trọng lượng"] = "4U", ["Độ cứng"] = "Trung bình" },
                        ImageUrl = PlaceholderSvg("4U", "#3b82f6")
                    }
                }
            },
            new Product
            {
                Name = "Vợt FeatherKick Lite",
                Slug = "vot-featherkick-lite",
                CategorySlug = "vot",
                Brand = "FeatherKick",
                Description = "Nhẹ và cân - giúp vung nhanh và giảm mỏi khi tập lâu.",
                DiscountPercent = 8,
                HeroImageUrl = PlaceholderSvg("LITE", "#f43f5e"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "3U - Nhẹ nhàng",
                        Price = 729000,
                        Stock = 22,
                        Attributes = new Dictionary<string, string> { ["Trọng lượng"] = "3U", ["Độ cứng"] = "Mềm" },
                        ImageUrl = PlaceholderSvg("3U", "#f43f5e")
                    },
                    new()
                    {
                        DisplayName = "4U - Đầm lực",
                        Price = 799000,
                        Stock = 18,
                        Attributes = new Dictionary<string, string> { ["Trọng lượng"] = "4U", ["Độ cứng"] = "Trung bình" },
                        ImageUrl = PlaceholderSvg("4U", "#3b82f6")
                    }
                }
            },

            // GIÀY
            new Product
            {
                Name = "Giày AeroStep Pro",
                Slug = "giay-aerostep-pro",
                CategorySlug = "giay",
                Brand = "AeroStep",
                Description = "Đệm êm - bám sân tốt, tối ưu cho di chuyển ngang.",
                DiscountPercent = 6,
                HeroImageUrl = PlaceholderSvg("AEROSTEP", "#3b82f6"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "Size 40 - Màu Đỏ",
                        Price = 990000,
                        Stock = 15,
                        Attributes = new Dictionary<string, string> { ["Size"] = "40", ["Màu"] = "Đỏ" },
                        ImageUrl = PlaceholderSvg("40", "#ef4444")
                    },
                    new()
                    {
                        DisplayName = "Size 42 - Màu Xanh",
                        Price = 990000,
                        Stock = 12,
                        Attributes = new Dictionary<string, string> { ["Size"] = "42", ["Màu"] = "Xanh" },
                        ImageUrl = PlaceholderSvg("42", "#3b82f6")
                    }
                }
            },
            new Product
            {
                Name = "Giày TurboGrip Max",
                Slug = "giay-turbogrip-max",
                CategorySlug = "giay",
                Brand = "TurboGrip",
                Description = "Gót chắc, đế bám - giảm trượt khi bứt tốc.",
                DiscountPercent = 7,
                HeroImageUrl = PlaceholderSvg("TURBO", "#ef4444"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "Size 41 - Đen",
                        Price = 1050000,
                        Stock = 10,
                        Attributes = new Dictionary<string, string> { ["Size"] = "41", ["Màu"] = "Đen" },
                        ImageUrl = PlaceholderSvg("41", "#111827")
                    },
                    new()
                    {
                        DisplayName = "Size 43 - Xanh",
                        Price = 1050000,
                        Stock = 8,
                        Attributes = new Dictionary<string, string> { ["Size"] = "43", ["Màu"] = "Xanh" },
                        ImageUrl = PlaceholderSvg("43", "#3b82f6")
                    }
                }
            },
            new Product
            {
                Name = "Giày SpeedRally II",
                Slug = "giay-speedrally-ii",
                CategorySlug = "giay",
                Brand = "SpeedRally",
                Description = "Trọng lượng vừa - nhẹ chân, thở tốt và êm khi tập luyện.",
                HeroImageUrl = PlaceholderSvg("RALLY II", "#0f172a"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "Size 40 - Xám",
                        Price = 930000,
                        Stock = 16,
                        Attributes = new Dictionary<string, string> { ["Size"] = "40", ["Màu"] = "Xám" },
                        ImageUrl = PlaceholderSvg("40", "#64748b")
                    },
                    new()
                    {
                        DisplayName = "Size 42 - Trắng",
                        Price = 930000,
                        Stock = 14,
                        Attributes = new Dictionary<string, string> { ["Size"] = "42", ["Màu"] = "Trắng" },
                        ImageUrl = PlaceholderSvg("42", "#e2e8f0")
                    }
                }
            },
            new Product
            {
                Name = "Giày CourtWave Comfort",
                Slug = "giay-courtwave-comfort",
                CategorySlug = "giay",
                Brand = "CourtWave",
                Description = "Thiết kế ôm chân - ổn định khi đổi hướng liên tục.",
                DiscountPercent = 10,
                HeroImageUrl = PlaceholderSvg("COMFORT", "#1d4ed8"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "Size 41 - Xanh",
                        Price = 980000,
                        Stock = 11,
                        Attributes = new Dictionary<string, string> { ["Size"] = "41", ["Màu"] = "Xanh" },
                        ImageUrl = PlaceholderSvg("41", "#3b82f6")
                    },
                    new()
                    {
                        DisplayName = "Size 44 - Đỏ",
                        Price = 1020000,
                        Stock = 7,
                        Attributes = new Dictionary<string, string> { ["Size"] = "44", ["Màu"] = "Đỏ" },
                        ImageUrl = PlaceholderSvg("44", "#ef4444")
                    }
                }
            },

            // QUẢ CẦU
            new Product
            {
                Name = "Quả cầu Feather76 Tourney (6 quả)",
                Slug = "cau-feather76-tourney-6",
                CategorySlug = "qua-cau",
                Brand = "Feather",
                Description = "Độ bay ổn định - phù hợp thi đấu và tập kỹ thuật.",
                DiscountPercent = 10,
                HeroImageUrl = PlaceholderSvg("CẦU 76/6", "#3b82f6"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "Feather - 76 - 6 quả",
                        Price = 160000,
                        Stock = 60,
                        Attributes = new Dictionary<string, string> { ["Tốc độ"] = "76", ["Loại"] = "Feather", ["Bộ"] = "6" },
                        ImageUrl = PlaceholderSvg("76-6", "#3b82f6")
                    },
                    new()
                    {
                        DisplayName = "Feather - 76 - 12 quả",
                        Price = 290000,
                        Stock = 35,
                        Attributes = new Dictionary<string, string> { ["Tốc độ"] = "76", ["Loại"] = "Feather", ["Bộ"] = "12" },
                        ImageUrl = PlaceholderSvg("76-12", "#ef4444")
                    }
                }
            },
            new Product
            {
                Name = "Quả cầu Feather78 Pro",
                Slug = "cau-feather78-pro",
                CategorySlug = "qua-cau",
                Brand = "Feather",
                Description = "Tốc độ cao - thích hợp đánh nhanh và phòng tập chuyên.",
                HeroImageUrl = PlaceholderSvg("CẦU 78", "#111827"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "Feather - 78 - 6 quả",
                        Price = 175000,
                        Stock = 50,
                        Attributes = new Dictionary<string, string> { ["Tốc độ"] = "78", ["Loại"] = "Feather", ["Bộ"] = "6" },
                        ImageUrl = PlaceholderSvg("78-6", "#111827")
                    },
                    new()
                    {
                        DisplayName = "Feather - 78 - 12 quả",
                        Price = 320000,
                        Stock = 28,
                        Attributes = new Dictionary<string, string> { ["Tốc độ"] = "78", ["Loại"] = "Feather", ["Bộ"] = "12" },
                        ImageUrl = PlaceholderSvg("78-12", "#3b82f6")
                    }
                }
            },
            new Product
            {
                Name = "Quả cầu Plastic76 Training",
                Slug = "cau-plastic76-training",
                CategorySlug = "qua-cau",
                Brand = "Training",
                Description = "Bền - hợp tập luyện lâu dài, giảm chi phí khi luyện phản xạ.",
                DiscountPercent = 5,
                HeroImageUrl = PlaceholderSvg("PLASTIC 76", "#ef4444"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "Plastic - 76 - 12 quả",
                        Price = 120000,
                        Stock = 80,
                        Attributes = new Dictionary<string, string> { ["Tốc độ"] = "76", ["Loại"] = "Plastic", ["Bộ"] = "12" },
                        ImageUrl = PlaceholderSvg("76-12", "#ef4444")
                    },
                    new()
                    {
                        DisplayName = "Plastic - 76 - 6 quả",
                        Price = 68000,
                        Stock = 70,
                        Attributes = new Dictionary<string, string> { ["Tốc độ"] = "76", ["Loại"] = "Plastic", ["Bộ"] = "6" },
                        ImageUrl = PlaceholderSvg("76-6", "#3b82f6")
                    }
                }
            },
            new Product
            {
                Name = "Quả cầu Feather76 Training",
                Slug = "cau-feather76-training",
                CategorySlug = "qua-cau",
                Brand = "Feather",
                Description = "Chất lượng bay tốt - phù hợp đánh đôi và tập chuyển động.",
                DiscountPercent = 8,
                HeroImageUrl = PlaceholderSvg("FEATHER 76", "#3b82f6"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "Feather - 76 - 6 quả",
                        Price = 145000,
                        Stock = 55,
                        Attributes = new Dictionary<string, string> { ["Tốc độ"] = "76", ["Loại"] = "Feather", ["Bộ"] = "6" },
                        ImageUrl = PlaceholderSvg("76-6", "#3b82f6")
                    },
                    new()
                    {
                        DisplayName = "Feather - 76 - 12 quả",
                        Price = 270000,
                        Stock = 30,
                        Attributes = new Dictionary<string, string> { ["Tốc độ"] = "76", ["Loại"] = "Feather", ["Bộ"] = "12" },
                        ImageUrl = PlaceholderSvg("76-12", "#ef4444")
                    }
                }
            },

            // QUẦN ÁO
            new Product
            {
                Name = "Áo thun SportAir",
                Slug = "ao-sportair",
                CategorySlug = "quan-ao",
                Brand = "SportAir",
                Description = "Vải thoáng - hút ẩm, mặc thoải mái khi tập cường độ cao.",
                DiscountPercent = 15,
                HeroImageUrl = PlaceholderSvg("AO THUN", "#ef4444"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "Size M - Màu Đỏ",
                        Price = 180000,
                        Stock = 40,
                        Attributes = new Dictionary<string, string> { ["Size"] = "M", ["Màu"] = "Đỏ" },
                        ImageUrl = PlaceholderSvg("M-DO", "#ef4444")
                    },
                    new()
                    {
                        DisplayName = "Size L - Màu Xanh",
                        Price = 180000,
                        Stock = 28,
                        Attributes = new Dictionary<string, string> { ["Size"] = "L", ["Màu"] = "Xanh" },
                        ImageUrl = PlaceholderSvg("L-XA", "#3b82f6")
                    }
                }
            },
            new Product
            {
                Name = "Áo polo DynamicDry",
                Slug = "ao-dynamicdry",
                CategorySlug = "quan-ao",
                Brand = "DynamicDry",
                Description = "Form gọn - thấm hút nhanh, tạo cảm giác khô ráo suốt trận đấu.",
                HeroImageUrl = PlaceholderSvg("POLO", "#1d4ed8"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "Size S - Màu Trắng",
                        Price = 220000,
                        Stock = 26,
                        Attributes = new Dictionary<string, string> { ["Size"] = "S", ["Màu"] = "Trắng" },
                        ImageUrl = PlaceholderSvg("S-TRANG", "#e2e8f0")
                    },
                    new()
                    {
                        DisplayName = "Size M - Màu Đen",
                        Price = 220000,
                        Stock = 19,
                        Attributes = new Dictionary<string, string> { ["Size"] = "M", ["Màu"] = "Đen" },
                        ImageUrl = PlaceholderSvg("M-DEN", "#111827")
                    }
                }
            },
            new Product
            {
                Name = "Quần short FlexMove",
                Slug = "quan-short-flexmove",
                CategorySlug = "quan-ao",
                Brand = "FlexMove",
                Description = "Co giãn tốt - ít nhăn, thuận lợi cho động tác bật nhảy và lùi nhanh.",
                DiscountPercent = 5,
                HeroImageUrl = PlaceholderSvg("SHORT", "#3b82f6"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "Size S - Màu Xanh",
                        Price = 160000,
                        Stock = 35,
                        Attributes = new Dictionary<string, string> { ["Size"] = "S", ["Màu"] = "Xanh" },
                        ImageUrl = PlaceholderSvg("S-XA", "#3b82f6")
                    },
                    new()
                    {
                        DisplayName = "Size M - Màu Đỏ",
                        Price = 160000,
                        Stock = 24,
                        Attributes = new Dictionary<string, string> { ["Size"] = "M", ["Màu"] = "Đỏ" },
                        ImageUrl = PlaceholderSvg("M-DO", "#ef4444")
                    }
                }
            },
            new Product
            {
                Name = "Áo khoác WindShield",
                Slug = "ao-khoac-windshield",
                CategorySlug = "quan-ao",
                Brand = "WindShield",
                Description = "Chống gió nhẹ, giữ ấm vừa đủ cho buổi tập đầu giờ hoặc cuối buổi.",
                DiscountPercent = 10,
                HeroImageUrl = PlaceholderSvg("WIND", "#111827"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "Size M - Màu Đen",
                        Price = 420000,
                        Stock = 18,
                        Attributes = new Dictionary<string, string> { ["Size"] = "M", ["Màu"] = "Đen" },
                        ImageUrl = PlaceholderSvg("M-DEN", "#111827")
                    },
                    new()
                    {
                        DisplayName = "Size XL - Màu Xanh",
                        Price = 450000,
                        Stock = 12,
                        Attributes = new Dictionary<string, string> { ["Size"] = "XL", ["Màu"] = "Xanh" },
                        ImageUrl = PlaceholderSvg("XL-XA", "#3b82f6")
                    }
                }
            },

            // PHỤ KIỆN
            new Product
            {
                Name = "GripMaster Bọc tay 0.65",
                Slug = "phu-kien-gripmaster-065",
                CategorySlug = "phu-kien",
                Brand = "GripMaster",
                Description = "Độ bám tốt, chống trượt mồ hôi. Dành cho người đánh cường độ cao.",
                DiscountPercent = 20,
                HeroImageUrl = PlaceholderSvg("GRIP", "#ef4444"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "0.65 - Màu Đỏ",
                        Price = 65000,
                        Stock = 120,
                        Attributes = new Dictionary<string, string> { ["Bề dày"] = "0.65", ["Màu"] = "Đỏ" },
                        ImageUrl = PlaceholderSvg("0.65", "#ef4444")
                    },
                    new()
                    {
                        DisplayName = "0.70 - Màu Xanh",
                        Price = 65000,
                        Stock = 90,
                        Attributes = new Dictionary<string, string> { ["Bề dày"] = "0.70", ["Màu"] = "Xanh" },
                        ImageUrl = PlaceholderSvg("0.70", "#3b82f6")
                    }
                }
            },
            new Product
            {
                Name = "Dây vợt StrungPower 22",
                Slug = "phu-kien-day-strungpower-22",
                CategorySlug = "phu-kien",
                Brand = "StrungPower",
                Description = "Độ đàn hồi ổn định - cảm giác đánh đều tay. Dành cho tập luyện thường xuyên.",
                DiscountPercent = 10,
                HeroImageUrl = PlaceholderSvg("STRING", "#3b82f6"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "Tension 22 - Màu Xám",
                        Price = 120000,
                        Stock = 45,
                        Attributes = new Dictionary<string, string> { ["Lực căng"] = "22", ["Màu"] = "Xám" },
                        ImageUrl = PlaceholderSvg("22", "#64748b")
                    },
                    new()
                    {
                        DisplayName = "Tension 24 - Màu Đen",
                        Price = 120000,
                        Stock = 38,
                        Attributes = new Dictionary<string, string> { ["Lực căng"] = "24", ["Màu"] = "Đen" },
                        ImageUrl = PlaceholderSvg("24", "#111827")
                    }
                }
            },
            new Product
            {
                Name = "Túi đựng JetBag",
                Slug = "phu-kien-tui-jetbag",
                CategorySlug = "phu-kien",
                Brand = "JetBag",
                Description = "Chống va đập nhẹ, nhiều ngăn - tiện mang vợt + đồ phụ.",
                HeroImageUrl = PlaceholderSvg("JTBAG", "#ef4444"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "Màu Đen",
                        Price = 260000,
                        Stock = 24,
                        Attributes = new Dictionary<string, string> { ["Màu"] = "Đen" },
                        ImageUrl = PlaceholderSvg("DEN", "#111827")
                    },
                    new()
                    {
                        DisplayName = "Màu Xanh",
                        Price = 260000,
                        Stock = 20,
                        Attributes = new Dictionary<string, string> { ["Màu"] = "Xanh" },
                        ImageUrl = PlaceholderSvg("XANH", "#3b82f6")
                    }
                }
            },
            new Product
            {
                Name = "Bình lắc JetShaker",
                Slug = "phu-kien-binh-lac-jetshaker",
                CategorySlug = "phu-kien",
                Brand = "JetShaker",
                Description = "Tiện mang shuttle để tập phản xạ nhanh.",
                DiscountPercent = 8,
                HeroImageUrl = PlaceholderSvg("SHAKER", "#111827"),
                Variants = new List<ProductVariant>
                {
                    new()
                    {
                        DisplayName = "6 lỗ - Trắng",
                        Price = 99000,
                        Stock = 70,
                        Attributes = new Dictionary<string, string> { ["Sức chứa"] = "6" , ["Màu"] = "Trắng" },
                        ImageUrl = PlaceholderSvg("6", "#e2e8f0")
                    },
                    new()
                    {
                        DisplayName = "12 lỗ - Đỏ",
                        Price = 129000,
                        Stock = 55,
                        Attributes = new Dictionary<string, string> { ["Sức chứa"] = "12" , ["Màu"] = "Đỏ" },
                        ImageUrl = PlaceholderSvg("12", "#ef4444")
                    }
                }
            }
        };
    }

    public IReadOnlyList<Category> Categories => _categories;
    public IReadOnlyList<Product> Products => _products;

    public Product? GetBySlug(string slug) =>
        _products.FirstOrDefault(p => p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));

    public string[] GetCategorySlugs() => _categories.Select(c => c.Slug).ToArray();

    private static string PlaceholderSvg(string label, string colorHex)
    {
        // Avoid large embedded images: render a simple SVG placeholder.
        var safeLabel = label.Length > 16 ? label.Substring(0, 16) : label;
        var svg =
            $"<svg xmlns='http://www.w3.org/2000/svg' width='800' height='520'>" +
            $"<defs><linearGradient id='g' x1='0' y1='0' x2='1' y2='1'><stop offset='0' stop-color='{colorHex}'/><stop offset='1' stop-color='#111827'/></linearGradient></defs>" +
            $"<rect width='100%' height='100%' fill='url(#g)'/>" +
            $"<text x='50%' y='50%' dominant-baseline='middle' text-anchor='middle' font-family='Arial' font-size='38' fill='white'>{safeLabel}</text>" +
            $"</svg>";

        return "data:image/svg+xml;utf8," + Uri.EscapeDataString(svg);
    }
}

