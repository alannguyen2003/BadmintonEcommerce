namespace BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public string Brand { get; set; }
    public bool Status { get; set; }
    public int TotalVariants { get; set; }
    public string Slug { get; set; }
    public string CategoryName { get; set; }
    public Guid CategoryId { get; set; }
    public PrimaryImageResponse PrimaryImage { get; set; }
}

public class PrimaryImageResponse
{
    public Guid Id { get; set; }
    public string Url { get; set; }
}