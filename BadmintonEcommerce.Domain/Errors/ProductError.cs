using BadmintonEcommerce.Domain.Abstraction.Errors;
using SharedKernel.Errors;

namespace BadmintonEcommerce.Domain.Errors;

public static class ProductError
{
    public static Error NotFound(Guid productId) => Error.NotFound(
        ProductErrorCommand.NotFound.Code,
        ProductErrorCommand.NotFound.Description + productId);

    public static Error NameMustNotBeEmpty() => Error.Problem(
        ProductErrorCommand.Validator.ProductName.NotEmpty,
        ProductErrorCommand.Validator.ProductName.NotEmpty);
    
    public static Error NameMustBeLessThan100Characters() => Error.Problem(
        ProductErrorCommand.Validator.ProductName.MaximumLength100,
        ProductErrorCommand.Validator.ProductName.MaximumLength100);
    
    public static Error DescriptionMustBeLessThan1000Characters() => Error.Problem(
        ProductErrorCommand.Validator.ProductDescription.MaximumLength1000,
        ProductErrorCommand.Validator.ProductDescription.MaximumLength1000);
    
    public static Error BrandMustNotBeEmpty() => Error.Problem(
        ProductErrorCommand.Validator.ProductBrand.NotEmpty,
        ProductErrorCommand.Validator.ProductBrand.NotEmpty);
    
    public static Error BrandMustBeLessThan100Characters() => Error.Problem(
        ProductErrorCommand.Validator.ProductBrand.MaximumLength100,
        ProductErrorCommand.Validator.ProductBrand.MaximumLength100);
}