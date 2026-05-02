using BadmintonEcommerce.Domain.Entities.Authentication;
using BadmintonEcommerce.Domain.Enums;
using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Domain.Entities.Order;

public class Order : Aggregate<Guid>
{
    public Guid CustomerId { get; set; }
    public Account Customer { get; set; }
    
    public string AbsoluteAddress { get; set; }
    public string AddressOptionalInformation { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public string AdditionalNotes { get; set; }
    
    public OrderStatusType Status { get; set; }
    
    //Collections
    public ICollection<OrderItem> OrderItems { get; set; }
}