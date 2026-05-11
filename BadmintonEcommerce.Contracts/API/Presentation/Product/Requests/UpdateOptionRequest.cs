namespace BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;

public class UpdateOptionRequest
{
    public List<AddedOptionRequest> AddedOptions { get; set; } = new List<AddedOptionRequest>();
    public List<Guid> DeletedOptionValues { get; set; }
    public List<UpdatedOptionValueRequest> UpdatedOptionValueRequests { get; set; } =
        new List<UpdatedOptionValueRequest>();
    
    public List<AddedVariantRequest> AddedVariants { get; set; }
    public List<UpdatedVariantRequest> UpdatedVariants { get; set; }
    
}

public class AddedOptionRequest
{
    public string Name { get; set; }
    public string Code { get; set; }
    public List<string> AddedValues { get; set; }
    public List<UpdatedOptionValueRequest> UpdatedValues { get; set; }
}

public class UpdatedOptionValueRequest 
{
    public Guid Id { get; set; }
    public string Value { get; set; }
}

public class AddedVariantRequest
{
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public List<OptionValueRequest> Values { get; set; }
}

public class UpdatedVariantRequest
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public List<OptionValueRequest> Values { get; set; }
}
