namespace SharedKernel.Errors;

public record Error
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    
    public static readonly Error NullValue = new (
        "General.Null",
        "Null value was provided",
        ErrorType.Failure);
    
    public string Code { get; set; }
    public string Description { get; set; }
    public ErrorType Type { get; set; }

    public Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    public static Error Failure(string code, string description) =>
        new Error(code, description, ErrorType.Failure);
    
    public static Error NotFound(string code, string description) => 
        new Error(code, description, ErrorType.NotFound);  
    
    public static Error Problem(string code, string description) =>
        new Error(code, description, ErrorType.Problem);
    
    public static Error Conflict(string code, string description) =>
        new Error(code, description, ErrorType.Conflict);
}