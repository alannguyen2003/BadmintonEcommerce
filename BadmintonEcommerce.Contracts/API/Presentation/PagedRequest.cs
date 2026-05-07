namespace BadmintonEcommerce.Contracts.API.Presentation;

public class PagedRequest<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public T Data { get; set; }
}