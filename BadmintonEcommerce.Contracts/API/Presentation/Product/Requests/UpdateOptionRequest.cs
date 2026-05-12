namespace BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;

public class UpdateOptionRequest
{
    public Guid ProductId { get; set; }
    public List<CreateOptionRequest> AddedOptions { get; set; } = new List<CreateOptionRequest>();
    public List<CreateVariantRequest> AddedVariants { get; set; }
    
    public List<Guid> DeletedOptionValues { get; set; }
    public List<Guid> DeletedVariants { get; set; }
    public List<Guid> DeletedOptions { get; set; }    
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
