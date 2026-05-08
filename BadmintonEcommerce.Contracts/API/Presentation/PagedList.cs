namespace BadmintonEcommerce.Contracts.API.Presentation;

public class PagedList<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public T Data { get; set; }
}